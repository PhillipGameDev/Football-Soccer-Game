using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelKeys;
    public Destructable kin;
    public GameObject kinUI;
    private bool isKinDestroied;
    private int deliveredKeys;
    private int playerKeyCount;
    private Player player;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    void OnEnable()
    {
        Key.OnKeyCollected += KeyCollected;
        Player.OnKeyDelivered += KeyDelivered;
        Destructable.OnDestroied += KinDestroied;
    }

    private void OnDisable()
    {
        Key.OnKeyCollected -= KeyCollected;
        Player.OnKeyDelivered -= KeyDelivered;
        Destructable.OnDestroied -= KinDestroied;
    }

    public void KeyCollected(int count)
    {
        playerKeyCount += count;
        Debug.Log("Keys " + playerKeyCount);
        SoundManager.Instance.Play(SoundManager.Instance.audioKeyPickup);
    }

    public void KeyDelivered(int count)
    {
        if (playerKeyCount > 0)
        {
            deliveredKeys += playerKeyCount;
            playerKeyCount = 0;
            Debug.Log("Delivered Keys  " + deliveredKeys);
            if (deliveredKeys >= levelKeys)
            {
                if (isKinDestroied)
                {
                    kinUI.SetActive(true);   
                    GameManager.singleton.AddKin(kin.type);
                }
                SoundManager.Instance.Play(SoundManager.Instance.audioEndOfLevel);
                player.Dance();
                Invoke("NextLevel", 2);
            }
        }
        else
        {
            SoundManager.Instance.Play(SoundManager.Instance.audioAngryTotem);
        }
    }

    public void KinDestroied(bool value)
    {
        isKinDestroied = true;
    }
    public void NextLevel()
    {
        Debug.Log("NextLevel");
        GameManager.singleton.NextLevel();
    }
}