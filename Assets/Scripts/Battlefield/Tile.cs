using Entities;
using UnityEngine;

namespace Battlefield
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] public  BaseUnit   unit;
        [SerializeField] private GameObject highlight;
        private                  bool       IsOccupied => unit != null;
    
        private void OnMouseEnter()
        {
            if(!IsOccupied)
                highlight.SetActive(true);
        }

        private void OnMouseExit()
        {
            highlight.SetActive(false);
        }
    }
}
