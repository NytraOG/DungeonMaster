using UnityEngine;

namespace Abilities
{
    public class AbilitybuttonScript : MonoBehaviour
    {
        public int         sortingIndex;
        public BaseAbility ability;

        public void ShowTooltip()
        {
            if(ability is null)
                return;

            var tooltipInstance = transform.parent
                                           .Find("Tooltip")
                                           .GetComponent<Tooltip>();

            tooltipInstance.gameObject.SetActive(true);
            tooltipInstance.RenderTooltip(ability.Tooltip);
        }

        public void HideTooltip()
        {
            var tooltipInstance = transform.parent
                                           .Find("Tooltip")
                                           .GetComponent<Tooltip>();

            tooltipInstance.gameObject.SetActive(false);
        }
    }
}