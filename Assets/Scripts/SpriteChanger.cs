using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite backSprite;
    public Sprite forwardSprite;
    public Sprite focusedSprite;
    public Sprite shockSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if ((spriteRenderer != null) && (defaultSprite != null)){
            spriteRenderer.sprite = defaultSprite;
        }
    }

    public void LeanBack()
    {
        if ((spriteRenderer != null) && (backSprite != null)){
            spriteRenderer.sprite = backSprite;
        }
    }

    public void LeanForward()
    {
        if ((spriteRenderer != null) && (forwardSprite != null)){
            spriteRenderer.sprite = forwardSprite;
        }
    }

    public void LoseFish()
    {
        if ((spriteRenderer != null) && (shockSprite != null)){
            spriteRenderer.sprite = shockSprite;
        }
    }

    public void Focus()
    {
        if ((spriteRenderer != null) && (focusedSprite != null)){
            spriteRenderer.sprite = focusedSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TouchDraw.horizontalSwipe += LeanForward;
        TouchDraw.verticalSwipe += LeanBack;
        // Method to change sprite (space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Focus();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoseFish();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            LeanForward();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            LeanBack();
        }

    }
}
