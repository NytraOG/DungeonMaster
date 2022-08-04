using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

namespace Services
{
    public class BattleService
    {
        private readonly GameObject             battleField;
        private          List<BaseUnit>         kampfteilnehmer;
        private          Dictionary<Guid, bool> warSchonDran;

        public BattleService(GameObject     battleField,
                             List<BaseUnit> kampfteilnehmer)
        {
            this.battleField     = battleField;
            this.kampfteilnehmer = kampfteilnehmer;
            WarSchonDranSetzen();
            InitiativeBestimmen();
        }

        private void WarSchonDranSetzen() => kampfteilnehmer.ForEach(kt => warSchonDran.Add(kt.Id, false));

        public void InitiativeBestimmen() => kampfteilnehmer = kampfteilnehmer.OrderByDescending(unit => unit.Initiative)
                                                                              .ToList();

        public void KampfrundeAbhandeln()
        {
            
        }

        public IEnumerable<BaseUnit> GetKampfteilnehmer() => kampfteilnehmer;

        private void Attack<TTarget, TAttacker>()
                where TTarget : BaseUnit
                where TAttacker : BaseUnit
        {
            var target   = battleField.GetComponent<TTarget>();
            var attacker = battleField.GetComponent<TAttacker>();

            if (target is not null && attacker is not null)
                attacker.DealDamage(target);
        }
    }
}