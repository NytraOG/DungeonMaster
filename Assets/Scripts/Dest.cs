using UnityEngine;

public class Dest : MonoBehaviour
{
    public void DestroyParent()
    {
        var parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}