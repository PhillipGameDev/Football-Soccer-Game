using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum KeyType { Air, Fire, Earth, Rock, Water }
public class Key : MonoBehaviour
{
    public static event UnityAction<int> OnKeyCollected;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("ON TRIGGER ENTER");
        if (other.CompareTag("Player"))
        {
            // TODO: Destroy key
            OnKeyCollected?.Invoke(1);
            Debug.Log("Key");
            Destroy(gameObject);
        }    
    }
}
