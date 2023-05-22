using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Defenseskill", menuName = "Skills/Defense")]
    public class DefenseSkill : SupportSkill
    {
        public override Factions TargetableFaction => Factions.None;
    }
}