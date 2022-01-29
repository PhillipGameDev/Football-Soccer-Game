using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualityMap : MonoBehaviour
{
    private List<GameObject> areas;

    void Start()
    {
        areas = new List<GameObject>();
        foreach(Transform child in transform)
        {
            areas.Add(child.gameObject);
        }
    }

    public void Switch(bool enabled)
    {
        foreach(GameObject area in areas)
        {
            area.SetActive(!area.activeSelf);
        }
    }
}
