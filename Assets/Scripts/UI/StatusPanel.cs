using Entities.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public  TextMeshProUGUI textMeleeDefense;
        public  TextMeshProUGUI textRangedDefense;
        public  TextMeshProUGUI textMagicDefense;
        public  TextMeshProUGUI textSocialDefense;
        public  TextMeshProUGUI textInitiativeBase;
        public  TextMeshProUGUI textHealthOrbPercentage;
        public  TextMeshProUGUI textManaOrbPercentage;
        public  Image           currentHealth;
        public  Image           currenMana;
        private float           healthSnapshot;
        private float           manaSnapshot;
        private Hero            selectedHero;

        private void Awake()
        {
            textHealthOrbPercentage.gameObject.SetActive(false);
            textManaOrbPercentage.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateOrbs();

            if (textName.text != selectedHero.name)
                textName.text = selectedHero.name;

            if (textStrength.text != selectedHero.Strength.ToString())
                textStrength.text = selectedHero.Strength.ToString();

            if (textConstitution.text != selectedHero.Constitution.ToString())
                textConstitution.text = selectedHero.Constitution.ToString();

            if (textDexterity.text != selectedHero.Dexterity.ToString())
                textDexterity.text = selectedHero.Dexterity.ToString();

            if (textQuickness.text != selectedHero.Quickness.ToString())
                textQuickness.text = selectedHero.Quickness.ToString();

            if (textIntuition.text != selectedHero.Intuition.ToString())
                textIntuition.text = selectedHero.Intuition.ToString();

            if (textLogic.text != selectedHero.Logic.ToString())
                textLogic.text = selectedHero.Logic.ToString();

            if (textWillpower.text != selectedHero.Willpower.ToString())
                textWillpower.text = selectedHero.Willpower.ToString();

            if (textWisdom.text != selectedHero.Wisdom.ToString())
                textWisdom.text = selectedHero.Wisdom.ToString();

            if (textMeleeDefense.text != selectedHero.ModifiedMeleeDefense.ToString("####"))
                textMeleeDefense.text = selectedHero.ModifiedMeleeDefense.ToString("####");

            if (textRangedDefense.text != selectedHero.ModifiedRangedDefense.ToString("####"))
                textRangedDefense.text = selectedHero.ModifiedRangedDefense.ToString("####");

            if (textMagicDefense.text != selectedHero.ModifiedMagicDefense.ToString("####"))
                textMagicDefense.text = selectedHero.ModifiedMagicDefense.ToString("####");

            if (textSocialDefense.text != selectedHero.ModifiedSocialDefense.ToString("####"))
                textSocialDefense.text = selectedHero.ModifiedSocialDefense.ToString("####");

            if (textInitiativeBase.text != selectedHero.BaseInitiative.ToString("####"))
                textInitiativeBase.text = selectedHero.BaseInitiative.ToString("####");
        }

        public void ShowPercentagesHealth() => textHealthOrbPercentage.gameObject.SetActive(true);

        public void ShowPercentagesMana() => textManaOrbPercentage.gameObject.SetActive(true);

        public void HidePercentagesHealth() => textHealthOrbPercentage.gameObject.SetActive(false);

        public void HidePercentagesMana() => textManaOrbPercentage.gameObject.SetActive(false);

        private void UpdateOrbs()
        {
            var healthUpdated = (int)selectedHero.CurrentHitpoints != (int)healthSnapshot;

            if (healthUpdated)
            {
                textHealthOrbPercentage.text = $"{(int)(selectedHero.CurrentHitpoints / selectedHero.MaximumHitpoints * 100)}%";
                currentHealth.fillAmount     = selectedHero.CurrentHitpoints / selectedHero.MaximumHitpoints;
                healthSnapshot               = selectedHero.CurrentHitpoints;
            }

            var manaUpdated = (int)selectedHero.CurrentMana != (int)manaSnapshot;

            if (manaUpdated)
            {
                textManaOrbPercentage.text = $"{(int)(selectedHero.CurrentMana / selectedHero.MaximumMana * 100)}%";
                currenMana.fillAmount      = selectedHero.CurrentMana / selectedHero.MaximumMana;
                manaSnapshot               = selectedHero.CurrentMana;
            }
        }

        public void ChangeHero(Hero hero) => selectedHero = hero;
    }
}