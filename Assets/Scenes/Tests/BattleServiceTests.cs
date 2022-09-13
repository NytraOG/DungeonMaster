using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Scenes.Tests
{
    public class BattleServiceTests
    {
        // private BattleService service;
        //
        // [SetUp]
        // public void Init()
        // {
        //     var kampfteilnehmer = new List<BaseUnit>();
        //     service = new BattleService(null, kampfteilnehmer);
        // }

        [Test]
        public void BattleServiceTestsSimplePasses()
        {
           // var asd = service.GetKampfteilnehmer();
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BattleServiceTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}