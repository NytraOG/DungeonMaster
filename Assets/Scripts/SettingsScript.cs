using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    public string mainMenuScene;

    // Start is called before the first frame updatec
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    public void BackToMenu() => SceneManager.LoadScene(mainMenuScene);
}