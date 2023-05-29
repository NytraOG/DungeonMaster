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

namespace Battlefield
{
    public struct BuffResolutionArgs
    {
        public BaseUnit Applicant            { get; set; }
        public Buff     Buff                 { get; set; }
        public string   Effect               { get; set; }
        public int      RemainingDuration    { get; set; }
        public Color    CombatlogEffectColor { get; set; }
    }

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
        public  GameObject                                               floatingCombatText;
        public  GameObject                                               battlefield;
        public  BaseHero                                                 selectedHero;
        public  List<BaseUnit>                                           selectedTargets;
        public  BaseSkill                                                selectedSkill;
        public  Material                                                 defaultMaterial;
        public  string                                                   skillDisabledColor;
        public  Material                                                 creatureOutlineMaterial;
        public  Material                                                 heroOutlineMaterial;
        public  List<BaseSkill>                                          skillsOfSelectedHero = new();
        public  bool                                                     abilityanzeigeIstAktuell;
        public  Text                                                     toastMessageText;
        public  Sprite                                                   originalButtonBackground;
        public  Sprite                                                   bloodPuddle;
        public  bool                                                     allesDa;
        public  List<Creature>                                           enemies          = new();
        public  List<AbilitySelection>                                   AbilitySelection = new();
        private bool                                                     combatActive;
        private List<BaseHero>                                           heroes = new();
        public  UnityAction<BaseUnit, BaseSkill, List<BaseUnit>, string> OnBuffApplied;
        public  UnityAction<BuffResolutionArgs>                          OnBuffTick;
        public  UnityAction<DebuffResolutionArgs>                        OnDebuffTick;
        public  UnityAction<CombatskillResolutionArgs>                   OnHit;
        public  UnityAction<string>                                      OnMisc;
        public  UnityAction<CombatskillResolutionArgs>                   OnMiss;
        private List<GameObject>                                         skillbuttons;

        public bool PlayerIsTargeting => selectedHero is not null &&
                                         selectedSkill is BaseDamageSkill { TargetableFaction: Factions.Foe }
                                                 or BaseTargetingSkill { TargetableFaction: Factions.All or Factions.Friend };

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
            else if (selectedSkill is not null && !selectedTargets.Any())
                ShowToast("No Targets selected");
            else if (selectedSkill is null)
                ShowToast("No Ability selected");
            else
            {
                selectedHero.InitiativeBestimmen();
                selectedHero.GetComponent<SpriteRenderer>().material = defaultMaterial;

                var targets = new List<BaseUnit>();

                var selection = new AbilitySelection
                {
                    Skill   = selectedSkill,
                    Actor   = selectedHero,
                    Targets = targets
                };

                if (selection.Skill is BaseTargetingSkill { autoTargeting: true, TargetableFaction: Factions.Foe } skill)
                {
                    var remainingTargetsAmount = skill.GetTargets(selection.Actor) - 1;

                    var remainingTargets = enemies.Except(selectedTargets)
                                                  .ToList();

                    for (var i = 0; i < remainingTargetsAmount; i++)
                    {
                        if (i >= remainingTargets.Count)
                            continue;

                        selectedTargets.Add(remainingTargets[i]);
                    }
                }

                targets.AddRange(selectedTargets);

                AbilitySelection.Add(selection);
            }

            selectedTargets.ForEach(t => t.GetComponent<SpriteRenderer>().material = defaultMaterial);
            selectedTargets.Clear();

            selectedSkill = null;
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

            if (abilityanzeigeIstAktuell || !skillbuttons.Any() || selectedHero is null)
                return;

            var counter = 0;

            while (counter < skillsOfSelectedHero.Count)
            {
                var skill = skillsOfSelectedHero[counter];

                skillbuttons[counter].GetComponent<Image>().sprite = skill.sprite;

                if (skill is BaseTargetingSkill tSkill && tSkill.Manacost > selectedHero.CurrentMana && ColorUtility.TryParseHtmlString(skillDisabledColor, out var color))
                    skillbuttons[counter].GetComponent<Image>().color = color;
                else
                    skillbuttons[counter].GetComponent<Image>().color = Color.white;

                skillbuttons[counter].GetComponent<Button>().enabled            = true;
                skillbuttons[counter].GetComponent<AbilitybuttonScript>().skill = skillsOfSelectedHero[counter];

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
                AbilitySelection = AbilitySelection.OrderByDescending(a => a.Actor.ModifiedInitiative)
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
                    else if (selection.Targets.Any() && selection.Targets.All(t => t.IsDead) && selection.Actor is Creature creature)
                    {
                        creature.SelectedSkill = null;
                        continue;
                    }
                    else if (selection.Targets.Any() && selection.Targets.All(t => t.IsDead)) { }
                    else
                    {
                        ProcessSkillactivation(selection);

                        yield return new WaitForSeconds(1f);
                    }

                    #region Resolve Debuffs

                    var debuffsToKill           = new List<Debuff>();
                    var stackableDebuffs        = selection.Actor.debuffs.Where(d => d.isStackable).ToList();
                    var groupedStackableDebuffs = stackableDebuffs.GroupBy(d => d.displayname);
                    var unstackableDebuffs      = selection.Actor.debuffs.Except(stackableDebuffs);

                    foreach (var debuffs in groupedStackableDebuffs)
                    {
                        var cumulatedDamage = ResolveEffect(debuffs, debuffsToKill, selection, out var remainingDuration);

                        OnDebuffTick?.Invoke(new DebuffResolutionArgs
                        {
                            Actor                = selection.Actor,
                            Debuff               = debuffs.First(),
                            Damage               = cumulatedDamage,
                            RemainingDuration    = remainingDuration,
                            CombatlogEffectColor = debuffs.First().combatlogEffectColor
                        });

                        if (cumulatedDamage <= 0)
                            continue;

                        ProcessFloatingCombatText(cumulatedDamage.ToString(), HitResult.None, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }

                    foreach (var debuff in unstackableDebuffs)
                    {
                        debuff.ResolveTick(selection.Actor);

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

                        if (debuff.damagePerTick <= 0)
                            continue;

                        ProcessFloatingCombatText(debuff.damagePerTick.ToString(), HitResult.None, selection.Actor);

                        yield return new WaitForSeconds(1f);
                    }

                    foreach (var debuff in debuffsToKill)
                        debuff.Die(selection.Actor);

                    #endregion

                    #region Resolve Buffs

                    var buffsToKill           = new List<Buff>();
                    var stackableBuffs        = selection.Actor.buffs.Where(b => b.isStackable).ToList();
                    var groupedStackableBuffs = stackableBuffs.GroupBy(b => b.displayname);
                    var unstackableBuffs      = selection.Actor.buffs.Except(stackableBuffs);

                    foreach (var buffs in groupedStackableBuffs)
                    {
                        buffs.ToList()
                             .ForEach(b =>
                              {
                                  b.ResolveTick(selection.Actor);

                                  if (b.DurationEnded)
                                      buffsToKill.Add(b);
                              });

                        OnBuffTick?.Invoke(new BuffResolutionArgs
                        {
                            Applicant            = selection.Actor,
                            Buff                 = buffs.First(),
                            Effect               = "EFFECT",
                            RemainingDuration    = buffs.Max(b => b.remainingDuration),
                            CombatlogEffectColor = buffs.First().combatlogEffectColor
                        });

                        ProcessFloatingCombatText($"{buffs.Key} TICK", HitResult.None, selection.Actor);
                    }

                    foreach (var buff in unstackableBuffs)
                    {
                        buff.ResolveTick(selection.Actor);

                        if (buff.DurationEnded)
                            buffsToKill.Add(buff);

                        OnBuffTick?.Invoke(new BuffResolutionArgs
                        {
                            Applicant            = selection.Actor,
                            Buff                 = buff,
                            Effect               = "EFFECT",
                            RemainingDuration    = buff.remainingDuration,
                            CombatlogEffectColor = buff.combatlogEffectColor
                        });

                        if (buff.remainingDuration >= 0)
                            ProcessFloatingCombatText($"{buff.name} TICK", HitResult.None, selection.Actor);
                    }

                    foreach (var buff in buffsToKill)
                        buff.Die(selection.Actor);

                    #endregion

                    if (selection.Actor.buffs.Any() || buffsToKill.Any())
                        yield return new WaitForSeconds(1f);

                    var skillsToRemove = new List<SupportSkill>();
                    var skillsToCheck  = new List<SupportSkill>();

                    foreach (var skill in selection.Actor.activeSkills)
                    {
                        if (skill.Value)
                        {
                            skill.Key.Reverse(selection.Actor);
                            skillsToRemove.Add(skill.Key);
                        }
                        else
                            skillsToCheck.Add(skill.Key);
                    }

                    foreach (var skill in skillsToCheck)
                        selection.Actor.activeSkills[skill] = true;

                    foreach (var skill in skillsToRemove)
                        selection.Actor.activeSkills.Remove(skill);
                }

                AbilitySelection.Clear();
            }

            heroes.ForEach(h => h.GetComponent<SpriteRenderer>().material = heroOutlineMaterial);

            combatActive = false;

            OnMisc?.Invoke("-----------------------------------------------------------------------------------------------");
        }

        private static int ResolveEffect(IGrouping<string, Debuff> debuffs, List<Debuff> debuffsToKill, AbilitySelection selection, out int remainingDuration)
        {
            var cumulatedDamage = debuffs.Sum(d => d.damagePerTick);
            remainingDuration = debuffs.Max(d => d.remainingDuration) - 1;

            debuffs.ToList()
                   .ForEach(d =>
                    {
                        d.remainingDuration--;

                        if (d.DurationEnded)
                            debuffsToKill.Add(d);
                    });

            selection.Actor.CurrentHitpoints -= cumulatedDamage;

            return cumulatedDamage;
        }

        private void ProcessSkillactivation(AbilitySelection selection)
        {
            switch (selection.Actor)
            {
                case Hero when selection.Skill is BaseDamageSkill damageSkill:
                    ResolveDamageSkill(selection, damageSkill);
                    break;
                case Hero hero when selection.Skill is SummonSkill summonSkill:
                    ResolveSummonSkill(selection, hero, summonSkill);
                    break;

                case Creature when selection.Skill is BaseDamageSkill foeDamageSkill:
                    ResolveDamageSkill(selection, foeDamageSkill);
                    break;
                case Creature creature when selection.Skill is SummonSkill foeSummonSkill:
                    ResolveSummonSkill(selection, creature, foeSummonSkill);
                    break;

                default:
                    UseSupportskill(selection);
                    break;
            }

            selection.Actor.CurrentMana -= selection.Skill.Manacost;
        }

        private void ResolveDamageSkill(AbilitySelection selection, BaseDamageSkill damageSkill)
        {
            foreach (var target in selection.Targets)
                damageSkill.Activate(selection.Actor, target);
        }

        private void ResolveSummonSkill(AbilitySelection selection, BaseUnit unit, SummonSkill summonSkill)
        {
            unit.UseAbility(summonSkill, HitResult.None);

            InstantiateFloatingCombatText(selection.Actor, $"<b>{summonSkill.displayName}</b>!");
        }

        private void UseSupportskill(AbilitySelection selection)
        {
            foreach (var target in selection.Targets)
            {
                var skillResult = selection.Actor.UseAbility(selection.Skill, HitResult.None, target) + " ";
                OnBuffApplied?.Invoke(selection.Actor, selection.Skill, selection.Targets, skillResult);

                ProcessFloatingCombatText(skillResult, HitResult.None, selection.Actor);
            }
        }

        private void Miss(AbilitySelection selection, int hitroll, BaseUnit target)
        {
            var skillResult = "MISS";

            if (selection.Actor is Creature creature)
                creature.SelectedSkill = null;

            OnMiss?.Invoke(new CombatskillResolutionArgs
            {
                Actor       = selection.Actor,
                Skill       = selection.Skill,
                Hitroll     = hitroll,
                Target      = target,
                Skillresult = skillResult
            });

            ProcessFloatingCombatText(skillResult, HitResult.None, target);
        }

        private void DealDamage(AbilitySelection selection, int hitroll, HitResult hitResult, BaseUnit target)
        {
            var skillResult = selection.Actor.UseAbility(selection.Skill, hitResult, target);

            OnHit?.Invoke(new CombatskillResolutionArgs
            {
                Actor       = selection.Actor,
                Skill       = selection.Skill,
                Hitroll     = hitroll,
                HitResult   = hitResult,
                Target      = target,
                Skillresult = skillResult
            });

            ProcessFloatingCombatText(skillResult, hitResult, target);
            ProcessDeath(target);
        }

        public void ProcessFloatingCombatText(string abilityResult, HitResult hitResult, BaseUnit target)
        {
            var wasDamage = int.TryParse(abilityResult, out var damage);

            if (wasDamage)
                InstantiateFloatingCombatText(target, hitResult, damage);
            else
                InstantiateFloatingCombatText(target, abilityResult);
        }

        public void ProcessDeath(BaseUnit target)
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

            var maxTargets = skill.GetTargets(creature) > enemies.Count ? enemies.Count : skill.GetTargets(creature);
            var retVal     = new List<BaseUnit>();

            var eligableTargets = new List<BaseUnit>();

            if (creature.SelectedSkill is SupportSkill supportSkill)
            {
                if (supportSkill.targetsWholeGroup)
                    maxTargets = enemies.Count;

                eligableTargets.AddRange(enemies.Where(e => !e.IsDead));
            }
            else
                eligableTargets.AddRange(heroes.Where(h => !h.IsDead));

            for (var i = 0; i < maxTargets; i++)
            {
                if (i < eligableTargets.Count)
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
            var skill = g.GetComponent<AbilitybuttonScript>().skill;

            if (selectedHero is not null && skill is BaseTargetingSkill tSkill && tSkill.Manacost <= selectedHero.CurrentMana)
            {
                selectedTargets.Clear();
                selectedSkill = skill;
            }
            else
                ShowToast("Not enough Mana");
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
        public BaseUnit  Actor       { get; set; }
        public BaseSkill Skill       { get; set; }
        public int       Hitroll     { get; set; }
        public HitResult HitResult   { get; set; }
        public BaseUnit  Target      { get; set; }
        public string    Skillresult { get; set; }
    }
}