using System.Globalization;
using Battlefield;
using UnityEngine;

namespace Abilities
{
    public class AbilitybuttonScript : MonoBehaviour
    {
        public int         sortingIndex;
        public BaseAbility ability;

        public void ShowTooltip()
        {
            if (ability is null)
                return;

            var tooltipInstance = transform.parent.transform.parent
                                           .Find("Tooltip")
                                           .GetComponent<Tooltip>();

            var controller = FindObjectOfType<BattleController>();
            var damage     = controller.selectedHero.GetApproximateDamage(ability);
            var tooltip    = ability.GetTooltip(int.Parse(damage.ToString(CultureInfo.InvariantCulture)));

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