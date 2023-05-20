using Battlefield;
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
                                           .Find("SkillTooltip")
                                           .GetComponent<Tooltip>();

            tooltipInstance.displayedSkill = this;
            var controller = FindObjectOfType<BattleController>();
            var damage     = controller.selectedHero.GetApproximateDamage(skill);
            var tooltip    = skill.GetTooltip(controller.selectedHero,$"{damage.Item1}-{damage.Item2}");

            tooltipInstance.gameObject.SetActive(true);
            tooltipInstance.RenderTooltip(tooltip);
        }

        public void HideTooltip()
        {
            var tooltipInstance = transform.parent.transform.parent
                                           .Find("SkillTooltip")
                                           .GetComponent<Tooltip>();

            tooltipInstance.gameObject.SetActive(false);
        }
    }
}