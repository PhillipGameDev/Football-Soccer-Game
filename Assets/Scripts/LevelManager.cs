using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    private int playerKeyCount;

    void OnEnable()
    {
        Player.OnKeyCollected += KeyCollected;
    }    

    private void OnDisable() 
    {
            Player.OnKeyCollected -= KeyCollected;
    }

    public void KeyCollected(int count)
    {
        playerKeyCount += count;
    }
}