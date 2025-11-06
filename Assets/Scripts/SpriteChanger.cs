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
    public Sprite defaultSprite_rodCast;
    public Sprite defaultSprite_rodUncast;
    public Sprite backSprite_rod;
    public Sprite forwardSprite_rod;
    public Sprite focusedSprite_rod;
    public Sprite shockSprite_rod;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if ((spriteRenderer != null) && (defaultSprite_rodUncast != null)){
            spriteRenderer.sprite = defaultSprite_rodUncast;
        }
    }

    public void Cast()
    {
        if ((spriteRenderer != null) && (defaultSprite_rodCast != null)){
            spriteRenderer.sprite = defaultSprite_rodCast;
        }
    }

    public void LeanBack()
    {
        if ((spriteRenderer != null) && (backSprite != null)){
            spriteRenderer.sprite = backSprite;
        }
    }

    public void LeanBackRod()
    {
        if ((spriteRenderer != null) && (backSprite_rod != null)){
            spriteRenderer.sprite = backSprite_rod;
        }
    }

    public void LeanForward()
    {
        if ((spriteRenderer != null) && (forwardSprite != null)){
            spriteRenderer.sprite = forwardSprite;
        }
    }

    public void LeanForwardRod()
    {
        if ((spriteRenderer != null) && (forwardSprite_rod != null)){
            spriteRenderer.sprite = forwardSprite_rod;
        }
    }

    public void LoseFishNoRod()
    {
        if ((spriteRenderer != null) && (shockSprite != null)){
            spriteRenderer.sprite = shockSprite;
        }
    }

    public void LoseFishRod()
    {
        if ((spriteRenderer != null) && (shockSprite_rod != null)){
            spriteRenderer.sprite = shockSprite_rod;
        }
    }

    public void Focus()
    {
        if ((spriteRenderer != null) && (focusedSprite != null)){
            spriteRenderer.sprite = focusedSprite;
        }
    }

    public void FocusRod()
    {
        if ((spriteRenderer != null) && (focusedSprite_rod != null)){
            spriteRenderer.sprite = focusedSprite_rod;
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
            FocusRod();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Cast();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoseFishRod();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            LeanForwardRod();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            LeanBackRod();
        }

    }
}
