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

        private void InitiativeBestimmen() => kampfteilnehmer = kampfteilnehmer.OrderByDescending(unit => unit.Initiative)
                                                                               .ToList();

        public void KampfrundeAbhandeln() { }

        public IEnumerable<BaseUnit> GetKampfteilnehmer() => kampfteilnehmer;


    }
}