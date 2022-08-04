using Entities;
using Entities.Classes;
using Entities.Enemies;
using Services;

namespace DungeonMaster.Tests.ServiceTests;

[TestClass]
public class BattleServiceTests
{
    private Assassin      kämpfer1;
    private Goblin        kämpfer2;
    private Goblin        kämpfer3;
    private BattleService service;

    [TestInitialize]
    public void Init()
    {
        kämpfer1 = new Assassin();
        kämpfer2 = new Goblin();
        kämpfer3 = new Goblin();

        var kämpfer = new BaseUnit[] { kämpfer1, kämpfer2, kämpfer3 };

        service = new BattleService(kämpfer);
    }

    [TestMethod]
    public void InitiativeBestimmen_DreiKampfteilnehmerMitUntershceidlichenInitiativen_ReihenfolgeRichtig()
    {
        // Arrange
        
        // Act
        service.InitiativeBestimmen();

        // Assert
        var results = service.GetKampfteilnehmer().ToList();
        
        var result1 = results.First();
        results.Remove(result1);

        var result2 = results.First();
        results.Remove(result2);

        var result3 = results.First();
        results.Remove(result3);
        
        Assert.IsTrue(result1.Id == kämpfer3.Id);
        Assert.IsTrue(result2.Id == kämpfer2.Id);
        Assert.IsTrue(result3.Id == kämpfer1.Id);
    }
}