using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using Entities.Hero;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace Battlefield
{
    public class BattleController : MonoBehaviour
    {
        public  GameObject                                         floatingCombatText;
        public  GameObject                                         battlefield;
        public  BaseHero                                           selectedHero;
        public  BaseUnit                                           selectedTarget;
        public  BaseSkill                                          selectedAbility;
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
        public  UnityAction<CombatskillResolutionArgs>             OnHit;
        public  UnityAction<string>                                OnMisc;
        public  UnityAction<CombatskillResolutionArgs>             OnMiss;
        private List<GameObject>                                   skillbuttons;

        private void Awake() => ConfigureAbilityButtons();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectedHero    = null;
                selectedTarget  = null;
                selectedAbility = null;
            }

            MachEnemiesCombatReady();
            SetupCombatants();
            AbilityspritesAuffrischen();
        }

        public void ConfirmAbilitySelection()
        {
            if (AbilitySelection.Any(s => s.Actor == selectedHero))
                ShowToast("Selected Hero is already acting");
            else if (selectedAbility is not null && selectedTarget is null)
                ShowToast("No Target selected");
            else if (selectedAbility is null)
                ShowToast("No Ability selected");
            else
            {
                selectedHero.InitiativeBestimmen();

                AbilitySelection.Add(new AbilitySelection
                {
                    Ability = selectedAbility,
                    Actor   = selectedHero,
                    Target  = selectedTarget
                });
            }

            selectedTarget  = null;
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
                skillbuttons[counter].GetComponent<AbilitybuttonScript>().skill = abilitiesOfSelectedHero[counter];
                skillbuttons[counter].GetComponent<Button>().enabled            = true;

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
                        yield return new WaitForSeconds(0.25f);
                    else if (selection.Actor.IsStunned)
                    {
                        OnMisc?.Invoke($"{selection.Actor.name}'s <b><color=yellow>Stun</color></b> expired");

                        InstantiateFloatingCombatText(selection.Actor, "STUNNED");

                        selection.Actor.IsStunned = false;

                        if (selection.Actor is Creature creature)
                            creature.SelectedSkill = null;

                        yield return new WaitForSeconds(0.5f);
                    }
                    else if (selection.Target.IsDead) { }
                    else
                    {
                        ProcessSkillactivation(selection, out var abilityResult);
                        ProcessDeath(selection);
                        ProcessFloatingCombatText(abilityResult, selection);

                        yield return new WaitForSeconds(0.5f);
                    }
                }

                AbilitySelection.Clear();
            }

            combatActive = false;
        }

        private void ProcessSkillactivation(AbilitySelection selection, out string abilityResult)
        {
            switch (selection)
            {
                case { Ability: BaseDamageSkill damageSkill, Target: Creature creature }:
                {
                    var isHit = CalculateHit(selection, damageSkill, creature, out var hitroll);

                    if (isHit)
                        DealDamage(selection, hitroll, out abilityResult);
                    else
                        Miss(selection, hitroll, out abilityResult);
                    break;
                }
                case { Ability: BaseDamageSkill foeDamageSkill, Target: BaseHero target, Actor: Creature creature }:
                {
                    var isHit = CalculateHit(selection, foeDamageSkill, target, out var hitroll);

                    if (isHit)
                        DealDamage(selection, hitroll, out abilityResult);
                    else
                    {
                        Miss(selection, hitroll, out abilityResult);

                        creature.SelectedSkill = null;
                    }

                    break;
                }
                default:
                    UseSupportskill(selection, out abilityResult);
                    break;
            }
        }

        private void UseSupportskill(AbilitySelection selection, out string abilityResult)
        {
            abilityResult = selection.Actor.UseAbility(selection.Ability, selection.Target);

            OnBuffApplied?.Invoke(selection.Actor, selection.Ability, selection.Target, abilityResult);
        }

        private void Miss(AbilitySelection selection, int hitroll, out string abilityResult)
        {
            abilityResult = "MISS";

            OnMiss?.Invoke(new CombatskillResolutionArgs
            {
                Actor         = selection.Actor,
                Skill         = selection.Ability,
                Hitroll       = hitroll,
                Target        = selection.Target,
                Abilityresult = abilityResult
            });
        }

        private void DealDamage(AbilitySelection selection, int hitroll, out string abilityResult)
        {
            abilityResult = selection.Actor.UseAbility(selection.Ability, selection.Target);

            OnHit?.Invoke(new CombatskillResolutionArgs
            {
                Actor         = selection.Actor,
                Skill         = selection.Ability,
                Hitroll       = hitroll,
                Target        = selection.Target,
                Abilityresult = abilityResult
            });
        }

        private static bool CalculateHit(AbilitySelection selection, BaseDamageSkill damageSkill, BaseUnit target, out int hitroll)
        {
            hitroll = damageSkill.GetHitroll(selection.Actor);

            return (int)(target.MeleeDefense * target.MeleeDefensmodifier) < hitroll;
        }

        private void ProcessFloatingCombatText(string abilityResult, AbilitySelection selection)
        {
            var wasDamage = int.TryParse(abilityResult, out var damage);

            if (wasDamage)
                InstantiateFloatingCombatText(selection.Target, damage);
            else
                InstantiateFloatingCombatText(selection.Target, abilityResult);
        }

        private void ProcessDeath(AbilitySelection selection)
        {
            if (selection.Target.IsDead)
            {
                selection.Target.transform.gameObject.GetComponent<SpriteRenderer>().sprite     = bloodPuddle;
                selection.Target.transform.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

        private void SetupCombatants()
        {
            if (allesDa)
                return;

            var battlefieldComponent = battlefield.GetComponent<BattleService>();

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
                    Ability = enemy.SelectedSkill,
                    Actor   = enemy,
                    Target  = heroes[rando.Next(0, heroes.Count)]
                });
            }
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

        private void InstantiateFloatingCombatText(BaseUnit unitInstance, int damageDealt)
        {
            var textcomponent = CreateTextComponent(unitInstance);

            if (unitInstance.CurrentHitpoints <= 0)
            {
                textcomponent.color = new Color(255, 0, 0);
                textcomponent.SetText($"({damageDealt}) Killing Blow!");
            }

            else
                textcomponent.SetText(damageDealt.ToString());
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
            selectedTarget  = null;
            selectedAbility = g.GetComponent<AbilitybuttonScript>().skill;
        }

        private void ShowToast(string text, int duration = 2) => StartCoroutine(ShowToastCore(text, duration));

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
        public BaseSkill Ability { get; set; }
        public BaseUnit  Target  { get; set; }
        public BaseUnit  Actor   { get; set; }
    }

    public struct CombatskillResolutionArgs
    {
        public BaseUnit  Actor         { get; set; }
        public BaseSkill Skill         { get; set; }
        public int       Hitroll       { get; set; }
        public BaseUnit  Target        { get; set; }
        public string    Abilityresult { get; set; }
    }
}