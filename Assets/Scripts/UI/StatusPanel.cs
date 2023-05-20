using Entities.Hero;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatusPanel : MonoBehaviour
    {
        public  TextMeshProUGUI textStrength;
        public  TextMeshProUGUI textConstitution;
        public  TextMeshProUGUI textDexterity;
        public  TextMeshProUGUI textQuickness;
        public  TextMeshProUGUI textIntuition;
        public  TextMeshProUGUI textLogic;
        public  TextMeshProUGUI textWillpower;
        public  TextMeshProUGUI textWisdom;
        public  TextMeshProUGUI textName;
        private bool            assignmentChanged;
        private Hero            selectedHero;

        private void Update()
        {
            if (!assignmentChanged)
                return;

            textName.text         = selectedHero.name;
            textStrength.text     = selectedHero.Strength.ToString();
            textConstitution.text = selectedHero.Constitution.ToString();
            textDexterity.text    = selectedHero.Dexterity.ToString();
            textQuickness.text    = selectedHero.Quickness.ToString();
            textIntuition.text    = selectedHero.Intuition.ToString();
            textLogic.text        = selectedHero.Logic.ToString();
            textWillpower.text    = selectedHero.Willpower.ToString();
            textWisdom.text       = selectedHero.Wisdom.ToString();

            assignmentChanged = false;
        }

        public void ChangeHero(Hero hero)
        {
            selectedHero      = hero;
            assignmentChanged = true;
        }
    }
}