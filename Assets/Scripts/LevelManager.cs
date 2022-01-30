using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelKeys;
    public GameObject kin;
    public GameObject kinUI;
    private bool isKinDestroied;
    private int deliveredKeys;
    private int playerKeyCount;

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
        Destructable.OnDestroied += KinDestroied;
    }

    public void KeyCollected(int count)
    {
        playerKeyCount += count;
        Debug.Log("Keys " + playerKeyCount);
    }

    public void KeyDelivered(int count)
    {
        if (playerKeyCount > 0)
        {
            deliveredKeys += playerKeyCount;
            playerKeyCount = 0;
            if (deliveredKeys == levelKeys)
            {
                if (isKinDestroied)
                {
                    kinUI.SetActive(true);
                    Debug.Log("Keys  " + playerKeyCount);
                    Invoke("NextLevel", 3);
                }else NextLevel();
            }
        }
        Debug.Log("Keys  " + playerKeyCount);
        Debug.Log("Totem " + deliveredKeys);
    }

    public void KinDestroied(bool value)
    {
        isKinDestroied = true;
    }
    public void NextLevel()
    {
        GameManager.singleton.NextLevel();
    }
}