using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Totem : MonoBehaviour
{
    [SerializeField] private TotemKeySpot[] keySpots;
    private List<Key> keys;

    private int collectedCount = 0;

    public int CollectedCount
    {
        get => collectedCount;
        set
        {
            collectedCount = value;

            if (collectedCount >= keys.Count)
            {
                OnAllKeyCollected?.Invoke();
            }
        }
    }

    public event UnityAction OnAllKeyCollected;

    void Start()
    {
        KeyPickup[] keyPickups = FindObjectsOfType<KeyPickup>();
        keys = new List<Key>();
        foreach(KeyPickup pickup in keyPickups)
        {
            keys.Add(pickup.Key);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            keySpots[i].Key = keys[i];
            keySpots[i].gameObject.SetActive(true);
        }
    }

    public void CollectKey(Key key)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].KeyType == key.KeyType && !keySpots[i].Collected)
            {
                keySpots[i].Collected = true;
                CollectedCount++;
                SoundManager.Instance.Play(SoundManager.Instance.audioKeyPickup);
            }
        }
    }

    public void NoKey()
    {
        SoundManager.Instance.Play(SoundManager.Instance.audioAngryTotem);

        for (int i = 0; i < keySpots.Length; i++)
        {
            keySpots[i].PlayNoKeyAnimation();
        }
    }
}
