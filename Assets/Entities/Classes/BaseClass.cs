namespace Entities.Classes
{
    public abstract class BaseClass : ScriptableObject
    {
        public abstract void ApplyModifiers<T>(T unit)
                where T : BaseUnit;

        public abstract void ApplyAbilities<T>(T unit)
                where T : BaseUnit;
    }
}