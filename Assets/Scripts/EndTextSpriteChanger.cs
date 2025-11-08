using UnityEngine;

public class EndTextSpriteChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite fishCaught;
    public Sprite fishLost;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if ((spriteRenderer != null) && (defaultSprite != null))
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    public void fishCaughtDisplay()
    {
        if ((spriteRenderer != null) && (fishCaught != null)){
            spriteRenderer.sprite = fishCaught;
        }
    }

    public void fishLostDisplay()
    {
        if ((spriteRenderer != null) && (fishLost != null)){
            spriteRenderer.sprite = fishLost;
        }
    }

    public void displayNothing()
    {
        if ((spriteRenderer != null) && (defaultSprite != null)){
            spriteRenderer.sprite = defaultSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        NoteManager.catchFish += fishCaughtDisplay;
        NoteManager.loseFish += fishLostDisplay;

    }
}
