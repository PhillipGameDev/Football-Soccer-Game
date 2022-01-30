using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public static event UnityAction<bool> OnDestroied;
    public KinType type;
    private void OnDisable()
    {
        OnDestroied?.Invoke(true);
    }
}
