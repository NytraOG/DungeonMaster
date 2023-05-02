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
        public  List<AbilitySelection> AbilitySelection = new();
        private bool                   allesDa;
        private List<BaseFoe>          enemies = new();
        private List<BaseHero>         heroes  = new();
        private List<GameObject>       skillbuttons;

        private void Awake() => ConfigureAbilityButtons();

        private void Update()
        {
            MachEnemiesCombatReady();
            SetupCombatants();
            AbilityspritesAuffrischen();
        }

        public void ConfirmAbilitySelection()
        {
            if (AbilitySelection.Any(s => s.Actor == selectedHero))
                ShowToast("Selected Hero is already acting");
            else
            {
                AbilitySelection.Add(new AbilitySelection
                {
                    Ability = selectedAbility,
                    Actor   = selectedHero,
                    Target  = selectedEnemy
                });
            }

            selectedEnemy   = null;
            selectedHero    = null;
            selectedAbility = null;
        }

        public void StartRound() => StartCoroutine(KampfrundeAbhandeln());

        private void AbilityspritesAuffrischen()
        {
            if (abilityanzeigeIstAktuell || !skillbuttons.Any())
                return;

            for (var i = 0; i < abilitiesOfSelectedHero.Count; i++)
            {
                skillbuttons[i].GetComponent<Image>().sprite                = abilitiesOfSelectedHero[i].sprite;
                skillbuttons[i].GetComponent<AbilitybuttonScript>().ability = abilitiesOfSelectedHero[i];
                skillbuttons[i].GetComponent<Button>().enabled              = true;
            }

            abilityanzeigeIstAktuell = true;
        }

        private IEnumerator KampfrundeAbhandeln()
        {
            if (!AbilitySelection.Any())
                ShowToast("No Abilities have been selected", 1);
            else
            {
                var rando  = new Random().Next(0, heroes.Count);
                var target = heroes[rando];

                foreach (var enemy in enemies)
                {
                    var damageDealt = enemy.UseAbility(enemy.SelectedAbility, target);
                    InstantiateFloatingCombatText(target, damageDealt);

                    yield return new WaitForSeconds(0.5f);
                }

                foreach (var selection in AbilitySelection)
                {
                    var damageDealt = selection.Actor.UseAbility(selection.Ability, selection.Target);
                    InstantiateFloatingCombatText(selection.Target, damageDealt);

                    yield return new WaitForSeconds(0.5f);
                }

                AbilitySelection.Clear();
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
            var notCombatReadyEnemies = enemies.Where(e => e.SelectedAbility is null);

            foreach (var enemy in notCombatReadyEnemies)
                enemy.PickAbility();
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
                textcomponent.SetText("Killing Blow!");
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

        private void SetSelectedAbility(GameObject g) => selectedAbility = g.GetComponent<AbilitybuttonScript>().ability;

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