using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void DestroyParent()
    {
        var parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}