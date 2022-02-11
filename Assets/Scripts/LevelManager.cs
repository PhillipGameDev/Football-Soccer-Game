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
    private Totem totem;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        totem = FindObjectOfType<Totem>();
        totem.OnAllKeyCollected += EndLevel;
    }

    void OnDestroy()
    {
        totem.OnAllKeyCollected -= EndLevel;
    }

    void OnEnable()
    {
        Destructable.OnDestroied += KinDestroied;
    }

    private void OnDisable()
    {
        Destructable.OnDestroied -= KinDestroied;
    }

    public void EndLevel()
    {
        if (isKinDestroied)
        {
            kinUI.SetActive(true);   
            GameManager.singleton.AddKin(kin.type);
        }

        SoundManager.Instance.Play(SoundManager.Instance.audioEndOfLevel, 0.4f);
        player.Dance();
        Invoke("NextLevel", 2);
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
                SoundManager.Instance.Play(SoundManager.Instance.audioEndOfLevel, 0.4f);
                player.Dance();
                Invoke("NextLevel", 2);
            }
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