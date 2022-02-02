using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public static event UnityAction<bool> OnDestroied;
    public KinType type;
    public GameObject kinObject;
    private Collider2D coll;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        kinObject.SetActive(false);
    }

    public void Break()
    {
        coll.enabled = false;
        anim.SetTrigger("break");
        
        SoundManager.Instance.Play(SoundManager.Instance.audioCrashingRocks);

        if (type != KinType.None)
        {
            kinObject.SetActive(true);
            SoundManager.Instance.Play(SoundManager.Instance.audioKinPickup, 0.4f);
            GameManager.singleton.AddKin(type);
        }

        

        OnDestroied?.Invoke(true);
    }

    private void OnBreakEnd()
    {
        Destroy(gameObject);
    }
}
