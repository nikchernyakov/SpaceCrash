using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Sprite background;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(background != null)
            spriteRenderer.sprite = background;
    }

    public void SetBackground(Sprite background)
    {
        this.background = background;
    }
    
    
}
