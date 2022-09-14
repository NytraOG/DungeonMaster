using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameScene;
    public string settingsScene;

    // Start is called before the first frame updatec
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    public void StartGame() => SceneManager.LoadScene(gameScene);

    public void OpenSettings() => SceneManager.LoadScene(settingsScene);
}