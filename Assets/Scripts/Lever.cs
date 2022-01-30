using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public bool isToggle;
    private bool toggleValue = false;
    public UnityEvent<bool> action;
    private float cooldownTime = 0.5f;
    private bool isInCooldown = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInCooldown)
        {
            return;
        }
        else
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
                    isInCooldown = true;
                    Invoke("SetCooldown", cooldownTime);
                }
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

    private void SetCooldown()
    {
        isInCooldown = false;
    }
}
