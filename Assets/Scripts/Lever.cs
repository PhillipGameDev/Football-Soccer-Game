using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public bool isToggle;
    private bool toggleValue = false;
    public UnityEvent<bool> action;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isToggle)
        {
            action.Invoke(true);
            Debug.Log("Pressed  " + true);
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                toggleValue = !toggleValue;
                action.Invoke(toggleValue);
                Debug.Log("Toggle  " + toggleValue);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isToggle)
        {
            action.Invoke(false);
            Debug.Log("Pressed  " + false);
        }

    }
}
