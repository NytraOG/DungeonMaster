using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Entities;
using Entities.Classes;
using Entities.Enemies;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Battlefield
{
    public class BattleController : MonoBehaviour
    {
        public  GameObject             floatingCombatText;
        public  GameObject             battlefield;
        public  BaseHero               selectedHero;
        public  BaseFoe                selectedEnemy;
        public  List<BaseAbility>      abilitiesOfSelectedHero = new();
        public  List<AbilitySelection> AbilitySelection        = new();
        private bool                   allesDa;
        private List<BaseFoe>          enemies = new();
        private List<BaseHero>         heroes  = new();

        private void Start() { }

        private void Update()
        {
            MachEnemiesCombatReady();
            SetupCombatants();
        }

        public void ConfirmAbilitySelection()
        {
            AbilitySelection.Add(new AbilitySelection
            {
                Ability = abilitiesOfSelectedHero[0], //TODO Die Ability des jeweiligen Buttons nutzen
                Actor   = selectedHero,
                Target  = selectedEnemy
            });

            selectedEnemy = null;
            selectedHero  = null;
        }

        public void StartRound() => StartCoroutine(KampfrundeAbhandeln());

        private IEnumerator KampfrundeAbhandeln()
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
            damageTextInstance.layer = 15;

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
    }

    public struct AbilitySelection
    {
        public BaseAbility Ability { get; set; }
        public BaseUnit    Target  { get; set; }
        public BaseUnit    Actor   { get; set; }
    }
}