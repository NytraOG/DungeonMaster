using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Entities;
using Entities.Classes;
using Entities.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Battlefield
{
    public class BattleController : MonoBehaviour
    {
        public  GameObject             floatingCombatText;
        public  GameObject             battlefield;
        public  BaseHero               selectedHero;
        public  BaseFoe                selectedEnemy;
        public  BaseAbility            selectedAbility;
        public  List<BaseAbility>      abilitiesOfSelectedHero = new();
        public  bool                   abilityanzeigeIstAktuell;
        public  Text                   toastMessageText;
        public  Sprite                 originalButtonBackground;
        public  Sprite                 bloodPuddle;
        public  List<AbilitySelection> AbilitySelection = new();
        private bool                   allesDa;
        private bool                   combatActive;
        private List<BaseFoe>          enemies = new();
        private List<BaseHero>         heroes  = new();
        private List<GameObject>       skillbuttons;

        private void Awake()
        {
            ConfigureAbilityButtons();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectedHero    = null;
                selectedEnemy   = null;
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
            else if (selectedAbility is not null && selectedEnemy is null)
                ShowToast("No Target selected");
            else if(selectedAbility is null)
                ShowToast("No Ability selected");
            else
            {
                selectedHero.InitiativeBestimmen();

                AbilitySelection.Add(new AbilitySelection
                {
                    Ability = selectedAbility,
                    Actor   = selectedHero,
                    Target  = selectedEnemy
                });
            }

            selectedEnemy   = null;
            selectedAbility = null;
        }

        public void StartRound() => StartCoroutine(KampfrundeAbhandeln());

        private void AbilityspritesAuffrischen()
        {
            if (selectedHero is null)
                skillbuttons.ForEach(b => b.GetComponent<Image>().sprite = originalButtonBackground);

            if (abilityanzeigeIstAktuell || !skillbuttons.Any())
                return;

            var counter = 0;

            while (counter < abilitiesOfSelectedHero.Count)
            {
                skillbuttons[counter].GetComponent<Image>().sprite                = abilitiesOfSelectedHero[counter].sprite;
                skillbuttons[counter].GetComponent<AbilitybuttonScript>().ability = abilitiesOfSelectedHero[counter];
                skillbuttons[counter].GetComponent<Button>().enabled              = true;

                counter++;
            }

            while (counter < skillbuttons.Count)
            {
                skillbuttons[counter].GetComponent<Image>().sprite   = originalButtonBackground;
                skillbuttons[counter].GetComponent<Button>().enabled = false;

                counter++;
            }

            abilityanzeigeIstAktuell = true;
        }

        private IEnumerator KampfrundeAbhandeln()
        {
            if (!AbilitySelection.Any())
                ShowToast("No Abilities have been selected", 1);
            else
            {
                combatActive = true;

                AbilitySelection = AbilitySelection.OrderByDescending(a => a.Actor.CurrentInitiative)
                                                   .ToList();

                foreach (var selection in AbilitySelection)
                {
                    if (!selection.Actor.IsDead)
                    {
                        var damageDealt = selection.Actor.UseAbility(selection.Ability, selection.Target);

                        if (selection.Target.IsDead)
                        {
                            selection.Target.transform.gameObject.GetComponent<SpriteRenderer>().sprite     = bloodPuddle;
                            selection.Target.transform.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                        }

                        if (damageDealt.HasValue)
                            InstantiateFloatingCombatText(selection.Target, damageDealt.Value);
                    }

                    yield return new WaitForSeconds(0.5f);
                }

                AbilitySelection.Clear();

                combatActive = false;
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
                                                           e.SelectedAbility is null);
            var rando = new Random();

            foreach (var enemy in notCombatReadyEnemies)
            {
                enemy.PickAbility();
                enemy.InitiativeBestimmen();

                AbilitySelection.Add(new AbilitySelection
                {
                    Ability = enemy.SelectedAbility,
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

            if (unitInstance.Hitpoints <= 0)
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
            selectedEnemy   = null;
            selectedAbility = g.GetComponent<AbilitybuttonScript>().ability;
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

        private IEnumerator MoveTextComponent(Text targetText, float duration)
        {
            var initialPosition = targetText.transform.position;
            var initX           = initialPosition.x;
            var initY           = initialPosition.y;
            var initZ           = initialPosition.z;
            var counter         = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;

                targetText.transform.position = new Vector3(initX, initY + counter);

                yield return null;
            }

            targetText.transform.position = new Vector3(initX, initY, initZ);
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
        public BaseAbility Ability { get; set; }
        public BaseUnit    Target  { get; set; }
        public BaseUnit    Actor   { get; set; }
    }
}