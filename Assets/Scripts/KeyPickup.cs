using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum KeyType { Air, Fire, Earth, Rock, Water }
public class KeyPickup : MonoBehaviour
{
    [SerializeField] private Key key;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Key Key { get => key; }

    // public static event UnityAction<int> OnKeyCollected;

    private void OnValidate()
    {
        if (key != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = key.Sprite;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // TODO: Destroy key
    //         OnKeyCollected?.Invoke(1);
    //         Debug.Log("Key");
    //         Destroy(gameObject);
    //     }    
    // }
}
