using System.Globalization;
using Battlefield;
using Skills.neu;
using UnityEngine;

namespace Skills
{
    public class AbilitybuttonScript : MonoBehaviour
    {
        public int       sortingIndex;
        public BaseSkill skill;

        public void ShowTooltip()
        {
            if (skill is null)
                return;

            var tooltipInstance = transform.parent.transform.parent
                                           .Find("Tooltip")
                                           .GetComponent<Tooltip>();

            var controller = FindObjectOfType<BattleController>();
            var damage     = controller.selectedHero.GetApproximateDamage(skill);
            var tooltip    = skill.GetTooltip(int.Parse(damage.ToString(CultureInfo.InvariantCulture)));

            tooltipInstance.gameObject.SetActive(true);
            tooltipInstance.RenderTooltip(tooltip);
        }

        public void HideTooltip()
        {
            var tooltipInstance = transform.parent.transform.parent
                                           .Find("Tooltip")
                                           .GetComponent<Tooltip>();

            tooltipInstance.gameObject.SetActive(false);
        }
    }
}