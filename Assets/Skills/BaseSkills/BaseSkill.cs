using Entities.Enums;
using UnityEngine;

namespace Skills.BaseSkills
{
    public abstract class BaseSkill : MonoBehaviour
    {
        public          string              Name           { get; set; }
        public          string              Description    { get; set; }
        public          int                 SkillLevel     { get; set; } //kommt ja eigentlich von woanders: Der Held/Gegner m�sste den Skilllevel definieren.
        public          float               SkillManaCost  { get; set; }
        public abstract SkillCategory       Category       { get; }
        public abstract SkillSubCategory    SubCategory    { get; }
        public abstract SkillDifficulty     Difficulty     { get; }
        public abstract SkillItemUsed       ItemUsed       { get; }
        public abstract SkillTargetPosition TargetPosition { get; }
        public abstract SkillRange          Range          { get; }
        public abstract SkillProvidedBy     ProvidedBy     { get; }
        public abstract SkillKeyword        Keyword        { get; }
    }
}