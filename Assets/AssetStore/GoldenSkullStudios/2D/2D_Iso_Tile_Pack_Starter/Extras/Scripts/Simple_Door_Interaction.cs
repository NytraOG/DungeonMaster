using UnityEngine;

namespace AssetStore.GoldenSkullStudios._2D._2D_Iso_Tile_Pack_Starter.Extras.Scripts
{
    public class Simple_Door_Interaction : MonoBehaviour
    {
        [SerializeField]
        private bool replaceDoorSprite = false;
        [SerializeField]
        private SpriteRenderer doorSpriteObject;
        [SerializeField]
        private Sprite doorOpenSprite;
        private Sprite doorCloseSprite;

        [SerializeField]
        private GameObject interactionTextObject;

        // Start is called before the first frame update
        void Start()
        {
            doorCloseSprite = doorSpriteObject.sprite;
            interactionTextObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if( replaceDoorSprite )
            {
                doorSpriteObject.sprite = doorOpenSprite;
            }

            interactionTextObject.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (replaceDoorSprite)
            {
                doorSpriteObject.sprite = doorCloseSprite;
            }
            interactionTextObject.SetActive(false);
        }
    }
}
