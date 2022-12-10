using Entities.Enemies;
using UnityEngine;

public class Kek : MonoBehaviour
{
    public GameObject enemy1;
    // Start is called before the first frame update
    private void Start()
    {
        Instantiate(enemy1, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(enemy1, new Vector3(1, 0, 0), Quaternion.identity);
        Instantiate(enemy1, new Vector3(2, 0, 0), Quaternion.identity);
        Instantiate(enemy1, new Vector3(3, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}