using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class MouseItemData : MonoBehaviour
    {
        public Image           itemSprite;
        public TextMeshProUGUI itemCount;

        private void Awake()
        {
            itemSprite.color = Color.clear;
            itemCount.text   = string.Empty;
        }

        // Start is called before the first frame update
        private void Start() { }

        // Update is called once per frame
        private void Update() { }
    }
}