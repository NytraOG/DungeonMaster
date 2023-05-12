using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Enemies;
using Entities.Hero;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Battlefield
{
    public class BattleController : MonoBehaviour
    {
        public  GameObject             combatLog;
        public  GameObject             logmessagePrefab;
        public  GameObject             floatingCombatText;
        public  GameObject             battlefield;
        public  BaseHero               selectedHero;
        public  BaseUnit               selectedTarget;
        public  BaseSkill              selectedAbility;
        public  List<BaseSkill>        abilitiesOfSelectedHero = new();
        public  bool                   abilityanzeigeIstAktuell;
        public  Text                   toastMessageText;
        public  Sprite                 originalButtonBackground;
        public  Sprite                 bloodPuddle;
        public  bool                   allesDa;
        public  List<BaseFoe>          enemies          = new();
        public  List<AbilitySelection> AbilitySelection = new();
        private bool                   combatActive;
        private List<BaseHero>         heroes = new();
        private List<GameObject>       skillbuttons;

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
                        InstantiateFloatingCombatText(selection.Actor, "STUNNED");
                        selection.Actor.IsStunned = false;

                        yield return new WaitForSeconds(0.5f);
                    }
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
                case { Ability: BaseDamageSkill damageSkill, Target: BaseFoe foe }:
                {
                    var isHit = CalculateHit(selection, damageSkill, foe);

                    if (isHit)
                        DealDamage(selection, out abilityResult);
                    else
                        LogMiss(selection, out abilityResult);
                    break;
                }
                case { Ability: BaseDamageSkill foeDamageSkill, Target: BaseHero target, Actor: BaseFoe actor}:
                {
                    var isHit = CalculateHit(selection, foeDamageSkill, target);

                    if (isHit)
                        DealDamage(selection, out abilityResult);
                    else
                    {
                        LogMiss(selection, out abilityResult);

                        actor.SelectedSkill = null;
                    }
                    break;
                }
                default:
                    LogSupportskillUsage(selection, out abilityResult);
                    break;
            }
        }

        private void LogSupportskillUsage(AbilitySelection selection, out string abilityResult)
        {
            abilityResult = selection.Actor.UseAbility(selection.Ability, selection.Target);

            Log($"[{(int)selection.Actor.CurrentInitiative}]{selection.Actor.name} used {selection.Ability.name} on {selection.Target.name}");
        }

        private void LogMiss(AbilitySelection selection, out string abilityResult)
        {
            abilityResult = "MISS";

            Log($"[{(int)selection.Actor.CurrentInitiative}]{selection.Actor.name}'s {selection.Ability.name} missed {selection.Target.name}.");
        }

        private void DealDamage(AbilitySelection selection, out string abilityResult)
        {
            abilityResult = selection.Actor.UseAbility(selection.Ability, selection.Target);

            Log($"[{(int)selection.Actor.CurrentInitiative}]{selection.Actor.name}'s {selection.Ability.name} hit {selection.Target.name} for {abilityResult} damage.");
        }

        private static bool CalculateHit(AbilitySelection selection, BaseDamageSkill damageSkill, BaseUnit target)
        {
            var hitroll = damageSkill.GetHitroll(selection.Actor);

            return (int)target.MeleeDefense < hitroll;
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
            var rando                 = new Random();

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

        private void Log(string message)
        {
            var logEntry   = Instantiate(logmessagePrefab, combatLog.transform);
            var textObject = logEntry.GetComponent<TextMeshProUGUI>();

            textObject.fontSize = 18;
            textObject.text     = message;
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
}