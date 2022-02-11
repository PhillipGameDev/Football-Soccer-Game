using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemKeySpot : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool collected;
    private Key key;

    public bool Collected
    {
        get => collected;
        set
        {
            if (collected != value)
            {
                collected = value;
                anim.SetTrigger("collected");
            }
        }
    }

    public Key Key
    {
        get => key;
        set
        {
            key = value;
            spriteRenderer.sprite = key.Sprite;
        }
    }

    public void PlayNoKeyAnimation() 
    {
        if (!Collected)
            anim.SetTrigger("noKey");
    }
}
