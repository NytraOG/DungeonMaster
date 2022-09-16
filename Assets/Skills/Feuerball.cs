public class Feuerball : BaseSkill
{
    public override skillCategory Category => skillCategory.Magic;

    public void Start() => Name = "Feuerball";
}