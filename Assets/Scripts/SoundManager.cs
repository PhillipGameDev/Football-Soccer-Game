using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource sourceEffects;

    public AudioClip audioDamage;
    public AudioClip audioDoubleJump;
    public AudioClip audioFire;
    public AudioClip audioJump;
    public AudioClip audioLightSwitch;
    public AudioClip audioTorchOn;
    public AudioClip audioTotemOpen;
    public AudioClip walkGrass;
    public AudioClip walkRock;
    public AudioClip walkSand;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            SoundManager.Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        if (sourceEffects == null)
            return;

        sourceEffects.PlayOneShot(clip);
    }
}
