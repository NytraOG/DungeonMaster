using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Buffs;
using Entities.Enemies;
using Entities.Enums;
using Entities.Hero;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace Battlefield
{
    public struct DebuffResolutionArgs
    {
        public BaseUnit Actor                { get; set; }
        public Debuff   Debuff               { get; set; }
        public int      Damage               { get; set; }
        public int      RemainingDuration    { get; set; }
        public Color    CombatlogEffectColor { get; set; }
    }

    public class BattleController : MonoBehaviour
    {
        public  GameObject                                         floatingCombatText;
        public  GameObject                                         battlefield;
        public  BaseHero                                           selectedHero;
        public  List<BaseUnit>                                     selectedTargets;
        public  BaseSkill                                          selectedAbility;
        public  Material                                           defaultMaterial;
        public  Material                                           creatureOutlineMaterial;
        public  Material                                           heroOutlineMaterial;
        public  List<BaseSkill>                                    abilitiesOfSelectedHero = new();
        public  bool                                               abilityanzeigeIstAktuell;
        public  Text                                               toastMessageText;
        public  Sprite                                             originalButtonBackground;
        public  Sprite                                             bloodPuddle;
        public  bool                                               allesDa;
        public  List<Creature>                                     enemies          = new();
        public  List<AbilitySelection>                             AbilitySelection = new();
        private bool                                               combatActive;
        private List<BaseHero>                                     heroes = new();
        public  UnityAction<BaseUnit, BaseSkill, BaseUnit, string> OnBuffApplied;
        public  UnityAction<DebuffResolutionArgs>                  OnDebuffTick;
        public  UnityAction<CombatskillResolutionArgs>             OnHit;
        public  UnityAction<string>                                OnMisc;
        public  UnityAction<CombatskillResolutionArgs>             OnMiss;
        private List<GameObject>                                   skillbuttons;

        private void Awake() => ConfigureAbilityButtons();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectedTargets.ForEach(t => t.GetComponent<SpriteRenderer>().material = defaultMaterial);
                selectedTargets.Clear();
            }

            MachEnemiesCombatReady();
            SetupCombatants();
            AbilityspritesAuffrischen();
        }

        public void ConfirmAbilitySelection()
        {
            if (AbilitySelection.Any(s => s.Actor == selectedHero))
                ShowToast("Selected Hero is already acting");
            else if (selectedAbility is not null && !selectedTargets.Any())
                ShowToast("No Targets selected");
            else if (selectedAbility is null)
                ShowToast("No Ability selected");
            else
            {
                selectedHero.InitiativeBestimmen();

                var targets = new List<BaseUnit>();
                targets.AddRange(selectedTargets);

                AbilitySelection.Add(new AbilitySelection
                {
                    Skill   = selectedAbility,
                    Actor   = selectedHero,
                    Targets = targets
                });
            }

            selectedTargets.ForEach(t => t.GetComponent<SpriteRenderer>().material = defaultMaterial);
            selectedTargets.Clear();

            selectedAbility = null;
        }

        public void StartRound() => StartCoroutine(KampfrundeAbhandeln());

        private void AbilityspritesAuffrischen()
        {
            if (selectedHero is null)
            {
                skillbuttons.ForEach(b =>
                {
                    b.GetComponent<Image>().sprite              = originalButtonBackground;
                    b.GetComponent<Button>().enabled            = false;
                    b.GetComponent<AbilitybuttonScript>().skill = null;
                });
            }

            if (abilityanzeigeIstAktuell || !skillbuttons.Any())
                return;

            var counter = 0;

            while (counter < abilitiesOfSelectedHero.Count)
            {
                skillbuttons[counter].GetComponent<Image>().sprite              = abilitiesOfSelectedHero[counter].sprite;
                skillbuttons[counter].GetComponent<Button>().enabled            = true;
                skillbuttons[counter].GetComponent<AbilitybuttonScript>().skill = abilitiesOfSelectedHero[counter];

                counter++;
            }

            while (counter < skillbuttons.Count)
            {
                skillbuttons[counter].GetComponent<Image>().sprite              = originalButtonBackground;
                skillbuttons[counter].GetComponent<Button>().enabled            = false;
                skillbuttons[counter].GetComponent<AbilitybuttonScript>().skill = null;

                counter++;
            }

            abilityanzeigeIstAktuell = true;
        }

        private IEnumerator KampfrundeAbhandeln()
        {
            combatActive = true;

            if (!AbilitySelection.Any())
                ShowToast("No Abilities have been selected", 1);
            else
            {
                AbilitySelection = AbilitySelection.OrderByDescending(a => a.Actor.CurrentInitiative)
                                                   .ToList();

                foreach (var selection in AbilitySelection)
                {
                    if (selection.Actor.IsDead)
                    {
                        yield return new WaitForSeconds(0.5f);
                        continue;
                    }

                    if (selection.Actor.IsStunned)
                    {
                        OnMisc?.Invoke($"{selection.Actor.name}'s <b><color=yellow>Stun</color></b> expired");

                        InstantiateFloatingCombatText(selection.Actor, "STUNNED");

                        selection.Actor.IsStunned = false;

                        if (selection.Actor is Creature creature)
                            creature.SelectedSkill = null;

                        yield return new WaitForSeconds(1f);
                    }
                    else if (selection.Skill is SummonSkill)
                    {
                        ProcessSkillactivation(selection, null, out var skillResult, out var hitResult);
                        ProcessFloatingCombatText(skillResult, hitResult, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }
                    else if (selection.Targets.All(t => t.IsDead) && selection.Actor is Creature creature)
                    {
                        creature.SelectedSkill = null;
                        continue;
                    }
                    else if (selection.Targets.All(t => t.IsDead)) { }
                    else if (selection.Skill is BaseTargetingSkill)
                    {
                        foreach (var target in selection.Targets)
                        {
                            ProcessSkillactivation(selection, target, out var skillResult, out var hitResult);
                            ProcessDeath(target);
                            ProcessFloatingCombatText(skillResult, hitResult, target);

                            yield return new WaitForSeconds(1f);
                        }
                    }
                    else
                    {
                        ProcessSkillactivation(selection, null, out var skillResult, out var hitResult);
                        ProcessFloatingCombatText(skillResult, hitResult, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }

                    var debuffsToKill = new List<Debuff>();

                    var stackableDebuffs        = selection.Actor.debuffs.Where(d => d.isStackable).ToList();
                    var groupedStackableDebuffs = stackableDebuffs.GroupBy(d => d.displayname);

                    var unstackableDebuffs = selection.Actor.debuffs.Except(stackableDebuffs);

                    foreach (var debuffs in groupedStackableDebuffs)
                    {
                        var cumulatedDamage   = debuffs.Sum(d => d.damagePerTick);
                        var remainingDuration = debuffs.Max(d => d.remainingDuration) - 1;

                        debuffs.ToList()
                               .ForEach(d =>
                                {
                                    d.remainingDuration--;

                                    if (d.DurationEnded)
                                        debuffsToKill.Add(d);
                                });

                        selection.Actor.CurrentHitpoints -= cumulatedDamage;

                        OnDebuffTick?.Invoke(new DebuffResolutionArgs
                        {
                            Actor                = selection.Actor,
                            Debuff               = debuffs.First(),
                            Damage               = cumulatedDamage,
                            RemainingDuration    = remainingDuration,
                            CombatlogEffectColor = debuffs.First().combatlogEffectColor
                        });

                        ProcessFloatingCombatText(cumulatedDamage.ToString(), HitResult.None, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }

                    foreach (var debuff in unstackableDebuffs)
                    {
                        debuff.DealDamage(selection.Actor);
                        debuff.remainingDuration--;

                        if (debuff.DurationEnded)
                            debuffsToKill.Add(debuff);

                        OnDebuffTick?.Invoke(new DebuffResolutionArgs
                        {
                            Actor                = selection.Actor,
                            Debuff               = debuff,
                            Damage               = debuff.damagePerTick,
                            RemainingDuration    = debuff.remainingDuration,
                            CombatlogEffectColor = debuff.combatlogEffectColor
                        });

                        ProcessFloatingCombatText(debuff.damagePerTick.ToString(), HitResult.None, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }

                    foreach (var debuff in debuffsToKill)
                    {
                        debuff.Reverse();

                        selection.Actor.debuffs.Remove(debuff);

                        Destroy(debuff);
                    }
                }

                AbilitySelection.Clear();
            }

            combatActive = false;

            OnMisc?.Invoke("-------------------------------------------------------------------------------------------");
        }

        private void ProcessSkillactivation(AbilitySelection selection, BaseUnit target, out string abilityResult, out HitResult hitResult)
        {
            switch (selection)
            {
                case { Actor: Hero, Skill: BaseDamageSkill damageSkill }:
                {
                    var isHit = CalculateHit(selection, damageSkill, target, out var hitroll, out var hitresult);

                    hitResult = hitresult;

                    if (isHit)
                        DealDamage(selection, hitroll, hitresult, target, out abilityResult);
                    else
                        Miss(selection, hitroll, target, out abilityResult);
                    break;
                }
                case { Actor: Creature creature, Skill: BaseDamageSkill foeDamageSkill }:
                {
                    var isHit = CalculateHit(selection, foeDamageSkill, target, out var hitroll, out var hitresult);

                    hitResult = hitresult;

                    if (isHit)
                        DealDamage(selection, hitroll, hitresult, target, out abilityResult);
                    else
                    {
                        Miss(selection, hitroll, target, out abilityResult);

                        creature.SelectedSkill = null;
                    }

                    break;
                }
                case { Actor: Creature creature, Skill: SummonSkill summonSkill }:
                {
                    hitResult     = HitResult.None;
                    abilityResult = creature.UseAbility(summonSkill, HitResult.None);

                    InstantiateFloatingCombatText(selection.Actor, $"<b>{summonSkill.displayName}</b>!");

                    creature.SelectedSkill = null;

                    break;
                }
                default:
                    UseSupportskill(selection, target, out abilityResult);
                    hitResult = HitResult.None;
                    break;
            }
        }

        private void UseSupportskill(AbilitySelection selection, BaseUnit target, out string abilityResult)
        {
            abilityResult = selection.Actor.UseAbility(selection.Skill, HitResult.None, target);

            OnBuffApplied?.Invoke(selection.Actor, selection.Skill, target, abilityResult);
        }

        private void Miss(AbilitySelection selection, int hitroll, BaseUnit target, out string abilityResult)
        {
            abilityResult = "MISS";

            OnMiss?.Invoke(new CombatskillResolutionArgs
            {
                Actor         = selection.Actor,
                Skill         = selection.Skill,
                Hitroll       = hitroll,
                Target        = target,
                Abilityresult = abilityResult
            });
        }

        private void DealDamage(AbilitySelection selection, int hitroll, HitResult hitResult, BaseUnit target, out string skillResult)
        {
            skillResult = selection.Actor.UseAbility(selection.Skill, hitResult, target);

            OnHit?.Invoke(new CombatskillResolutionArgs
            {
                Actor         = selection.Actor,
                Skill         = selection.Skill,
                Hitroll       = hitroll,
                HitResult     = hitResult,
                Target        = target,
                Abilityresult = skillResult
            });
        }

        private static bool CalculateHit(AbilitySelection selection, BaseDamageSkill damageSkill, BaseUnit target, out int hitroll, out HitResult hitResult)
        {
            hitroll = damageSkill.GetHitroll(selection.Actor);

            var relation = selection.Skill switch
            {
                MeleeSkill => hitroll / target.ModifiedMeleeDefense,
                RangedSkill => hitroll / target.ModifiedRangedDefense,
                MagicSkill => hitroll / target.ModifiedMagicDefense,
                _ => 0f
            };

            hitResult = relation switch
            {
                >= 2f => HitResult.Critical,
                >= 1.5f => HitResult.Good,
                >= 1f => HitResult.Normal,
                _ => HitResult.None
            };

            return (int)target.ModifiedMeleeDefense < hitroll;
        }

        private void ProcessFloatingCombatText(string abilityResult, HitResult hitResult, BaseUnit target)
        {
            var wasDamage = int.TryParse(abilityResult, out var damage);

            if (wasDamage)
                InstantiateFloatingCombatText(target, hitResult, damage);
            else
                InstantiateFloatingCombatText(target, abilityResult);
        }

        private void ProcessDeath(BaseUnit target)
        {
            if (target.IsDead)
            {
                var spriteRenderer = target.transform.gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite                                                 = bloodPuddle;
                spriteRenderer.material                                               = defaultMaterial;
                target.transform.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

        private void SetupCombatants()
        {
            if (allesDa)
                return;

            var battlefieldComponent = battlefield.GetComponent<SpawnController>();

            enemies = battlefieldComponent.enemies;
            heroes  = FindObjectsOfType<BaseHero>().ToList();

            allesDa = true;
        }

        private void MachEnemiesCombatReady()
        {
            if (combatActive)
                return;

            var notCombatReadyEnemies = enemies.Where(e => !e.IsDead &&
                                                           e.SelectedSkill is null);
            var rando = new Random();

            foreach (var enemy in notCombatReadyEnemies)
            {
                enemy.PickSkill();
                enemy.InitiativeBestimmen();

                AbilitySelection.Add(new AbilitySelection
                {
                    Skill   = enemy.SelectedSkill,
                    Actor   = enemy,
                    Targets = FindTargets(enemy)
                });
            }
        }

        private List<BaseUnit> FindTargets(Creature creature)
        {
            if (creature.SelectedSkill is not BaseTargetingSkill skill)
                return new List<BaseUnit>();

            var maxTargets = skill.GetTargets(creature);
            var retVal     = new List<BaseUnit>();

            var eligableTargets = heroes.Where(h => !h.IsDead)
                                        .ToList();

            for (var i = 0; i < maxTargets; i++)
            {
                if (i <= eligableTargets.Count)
                    retVal.Add(eligableTargets[i]);
            }

            return retVal;
        }

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, string combatText)
        {
            var textcomponent = CreateTextComponent(unitInstance);
            textcomponent.SetText(combatText);
        }

        private TextMeshPro CreateTextComponent(BaseUnit unitInstance)
        {
            var damageTextInstance = Instantiate(floatingCombatText, unitInstance.transform);

            var textcomponent = damageTextInstance.transform
                                                  .GetChild(0)
                                                  .GetComponent<TextMeshPro>();

            return textcomponent;
        }

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, HitResult hitResult, int damageDealt)
        {
            var textcomponent = CreateTextComponent(unitInstance);

            if (unitInstance.CurrentHitpoints <= 0)
            {
                textcomponent.color = new Color(255, 0, 0);
                textcomponent.SetText($"({damageDealt}) Killing Blow!");
            }
            else
            {
                textcomponent.color = ColorUtility.TryParseHtmlString(hitResult switch
                {
                    HitResult.None => Konstanten.NormalDamageColor,
                    HitResult.Normal => Konstanten.NormalDamageColor,
                    HitResult.Good => Konstanten.GoodDamageColor,
                    HitResult.Critical => Konstanten.CriticalDamageColor,
                    _ => throw new ArgumentOutOfRangeException(nameof(hitResult), hitResult, null)
                }, out var unityColor) ?
                        unityColor :
                        Color.white;

                var damageText = hitResult switch
                {
                    HitResult.Critical => $"{damageDealt}!!",
                    HitResult.Good => $"{damageDealt}!",
                    _ => damageDealt.ToString()
                };

                textcomponent.SetText(damageText);
            }
        }

        private void ConfigureAbilityButtons()
        {
            skillbuttons = GameObject.FindGameObjectsWithTag("Skillbutton")
                                     .OrderBy(b => b.GetComponent<AbilitybuttonScript>().sortingIndex)
                                     .ToList();

            skillbuttons.ForEach(g =>
            {
                g.GetComponent<Button>()
                 .onClick
                 .AddListener(() => SetSelectedAbility(g));
            });
        }

        private void SetSelectedAbility(GameObject g)
        {
            selectedTargets.Clear();
            selectedAbility = g.GetComponent<AbilitybuttonScript>().skill;
        }

        public void ShowToast(string text, int duration = 2) => StartCoroutine(ShowToastCore(text, duration));

        private IEnumerator ShowToastCore(string text,
                                          int    duration)
        {
            var txt = toastMessageText.GetComponent<Text>();
            txt.fontStyle = FontStyle.Bold;
            var orginalColor = txt.color;

            txt.text    = text;
            txt.enabled = true;

            //Fade in
            yield return fadeInAndOut(txt, true, 0.25f);
            //yield return MoveTextComponent(txt, 5);

            //Wait for the duration
            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            //Fade out
            yield return fadeInAndOut(txt, false, 2f);

            txt.enabled = false;
            txt.color   = orginalColor;
        }

        private IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
        {
            //Set Values depending on if fadeIn or fadeOut
            float a, b;

            if (fadeIn)
            {
                a = 0f;
                b = 1f;
            }
            else
            {
                a = 1f;
                b = 0f;
            }

            var currentColor = Color.red;
            var counter      = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                var alpha = Mathf.Lerp(a, b, counter / duration);

                targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

                yield return null;
            }
        }
    }

    public struct AbilitySelection
    {
        public BaseSkill      Skill   { get; set; }
        public List<BaseUnit> Targets { get; set; }
        public BaseUnit       Actor   { get; set; }
    }

    public struct CombatskillResolutionArgs
    {
        public BaseUnit  Actor         { get; set; }
        public BaseSkill Skill         { get; set; }
        public int       Hitroll       { get; set; }
        public HitResult HitResult     { get; set; }
        public BaseUnit  Target        { get; set; }
        public string    Abilityresult { get; set; }
    }
}