using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Image moon;
    public Image night;
    public Image sun;
    public Image world;

    void Start()
    {
        if (!GameManager.singleton.GetKins().Contains(KinType.Moon))
        {
            moon.color = new Color32(255, 255, 255, 80);
        }
        if (!GameManager.singleton.GetKins().Contains(KinType.Night))
        {
            night.color = new Color32(255, 255, 255, 80);
        }
        if (!GameManager.singleton.GetKins().Contains(KinType.Sun))
        {
            sun.color = new Color32(255, 255, 255, 80);
        }
        if (!GameManager.singleton.GetKins().Contains(KinType.World))
        {
            world.color = new Color32(255, 255, 255, 80);
        }
        Invoke("NextLevel",5);
    }
    
    public void NextLevel()
    {
        Debug.Log("NextLevel");
        GameManager.singleton.NextLevel();
    }
}
