using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string henlo;

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    public void LetsGo() => SceneManager.LoadScene(henlo);
}