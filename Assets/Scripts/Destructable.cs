using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public static event UnityAction<bool> OnDestroied;
    public KinType type;
    public GameObject kinObject;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        kinObject.SetActive(false);
    }

    public void Break()
    {
        anim.SetTrigger("break");
        SoundManager.Instance.Play(SoundManager.Instance.audioCrashingRocks);

        if (type != KinType.None)
        {
            kinObject.SetActive(true);
            SoundManager.Instance.Play(SoundManager.Instance.audioKinPickup);
        }

        

        OnDestroied?.Invoke(true);
    }

    private void OnBreakEnd()
    {
        Destroy(gameObject);
    }
}
