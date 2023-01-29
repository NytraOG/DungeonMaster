using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    private string settingsScene;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void ExitGame() => Application.Quit();

    public void OpenSettings() => SceneManager.LoadScene(settingsScene);
}
