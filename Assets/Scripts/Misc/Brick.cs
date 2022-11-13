using UnityEngine;
public class Brick : MonoBehaviour
{
   [SerializeField] Sprite newSprite;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        defaultSprite = spriteRenderer.sprite;
    }
    public void DisableGameObject()
    {
        transform.position = Vector3.zero;
        spriteRenderer.sprite = defaultSprite;
       gameObject.SetActive(false);
    }
    public void ChangeSprite()
    {
        spriteRenderer.sprite = newSprite;
    }
}
