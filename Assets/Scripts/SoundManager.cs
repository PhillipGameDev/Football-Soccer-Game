using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource sourceEffects;
    [SerializeField] private AudioSource sourceBackgroundEffects;

    public AudioClip audioAngryTotem;
    public AudioClip audioCrashingRocks;
    public AudioClip audioDamage;
    public AudioClip audioDoubleJump;
    public AudioClip audioEndOfLevel;
    public AudioClip audioError;
    public AudioClip audioFire;
    public AudioClip audioJump;
    public AudioClip audioKinPickup;
    public AudioClip audioLightSwitch;
    public AudioClip audioPushingRock;
    public AudioClip audioRockCrash;
    public AudioClip audioSwordAttack;
    public AudioClip audioTorchOn;
    public AudioClip audioTorchOff;
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

    public void SetBackgroundEffect(AudioClip clip, bool playing)
    {
        sourceBackgroundEffects.clip = clip;

        if (playing)
        {
            if(!sourceBackgroundEffects.isPlaying)
                sourceBackgroundEffects.Play();
        }
        else
        {
            if (sourceBackgroundEffects.isPlaying)
            {
                sourceBackgroundEffects.Stop();
            }
        }
    }

    public void StopBackground()
    {
        sourceBackgroundEffects.Stop();
        sourceBackgroundEffects.clip = null;
    }
}
