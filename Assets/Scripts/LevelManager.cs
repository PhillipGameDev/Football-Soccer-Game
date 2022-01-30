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
        Destructable.OnDestroied -= KinDestroied;
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
            Debug.Log("Keys  " + deliveredKeys);
            if (deliveredKeys == levelKeys)
            {
                if (isKinDestroied)
                {
                    kinUI.SetActive(true);
                    Invoke("NextLevel", 2);
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
        Debug.Log("NextLevel");
        GameManager.singleton.NextLevel();
    }
}