using UnityEngine;

public class SpriteCreator : MonoBehaviour
{
    public Texture2D texture;
    private Sprite mySprite;
    private SpriteRenderer myRenderer;  // Got all this from official unity documentation
    private void Awake()
    {
        myRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        myRenderer.color = new Color(0.9f, .9f, 0.9f, 1.0f);

        transform.position = new Vector3(1.5f, 1.5f, 0.0f);
    }
    private void Start()
    {
        //Texture2D texture = new Texture2D(64, 64);

        //Sprite sprite = Sprite.Create(texture, 
        //    new Rect(0, 0, texture.width, texture.height),
        //    Vector2.zero, 100, 0, SpriteMeshType.FullRect, 
        //    Vector4.zero, false);
        mySprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    private void OnGUI()
    {
        myRenderer.sprite = mySprite;
    }
}
