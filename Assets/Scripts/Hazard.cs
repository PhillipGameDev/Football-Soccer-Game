using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    private int scene_idx;
    private float delay = 1;
    // Start is called before the first frame update
    void Start()
    {
        scene_idx = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            //Might bypass loading scene by teleporting player to start of scene

            other.GetComponent<Player>().Die();
            Invoke("Reload", delay);
        }
    }
    
    private void Reload()
    {
        SceneManager.LoadScene(scene_idx);
    }
}
