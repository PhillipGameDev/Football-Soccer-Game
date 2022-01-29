using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    public int levelKeys;
    private int deliveredKeys;
    private int playerKeyCount;

    void OnEnable()
    {
        Key.OnKeyCollected += KeyCollected;
        Player.OnKeyDelivered += KeyDelivered;
    }    

    private void OnDisable() 
    {
            Key.OnKeyCollected -= KeyCollected;
            Player.OnKeyDelivered -= KeyDelivered;
    }

    public void KeyCollected(int count)
    {
        playerKeyCount += count;
        Debug.Log("Keys " + playerKeyCount);
    }

    public void KeyDelivered(int count)
    {
        if(playerKeyCount > 0){
            deliveredKeys += playerKeyCount;
            playerKeyCount = 0;
            if(deliveredKeys == levelKeys){
                GameManager.singleton.NextLevel();
            }
        }
        Debug.Log("Keys  " + playerKeyCount);
        Debug.Log("Totem " + deliveredKeys);
    }
}