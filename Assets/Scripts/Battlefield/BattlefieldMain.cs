using Entities.Classes;
using Entities.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlefieldMain : MonoBehaviour
{
    public string mainMenuScene;

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.AddComponent<Assassin>();
        gameObject.AddComponent<Goblin>();
    }

    // Update is called once per frame
    private void Update()
    {
        var gobbo = GetComponent<Goblin>();
        var ass   = GetComponent<Assassin>();

        if (ass is not null && ass.Lebenspunkte <= 0)
        {
            Debug.Log("Assassin is ded");
            Destroy(ass);
        }

        if (gobbo is not null && gobbo.Lebenspunkte <= 0)
        {
            Debug.Log("Gobbo is ded");
            Destroy(gobbo);
            foe.enabled = false;
        }
    }

    public void FoeAttack() { }

    public void AllyAttack() { }

    public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);

    #region Field

    public SpriteRenderer foe;
    public SpriteRenderer ally;
    public SpriteRenderer allyFront;
    public SpriteRenderer allyLeft;
    public SpriteRenderer allyRight;
    public SpriteRenderer allyMiddle;
    public SpriteRenderer allyBack;
    public SpriteRenderer allyAmbush;
    public SpriteRenderer foeFront;
    public SpriteRenderer foeRight;
    public SpriteRenderer foeLeft;
    public SpriteRenderer foeMiddle;
    public SpriteRenderer foeBack;
    public SpriteRenderer foeAmbush;

    #endregion
}