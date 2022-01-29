using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton = null;
    public Object[] scenes;
    public int current_scene = 0;
    private List<string> scenes_paths;
    void Awake()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            scenes_paths = new List<string>();
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes_paths.Add(AssetDatabase.GetAssetPath(scenes[i]));
            }
            GameManager.singleton = this;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        this.current_scene += 1;
        if (current_scene < this.scenes_paths.Count)
        {
            SceneManager.LoadScene(scenes_paths[this.current_scene]);
        }
    }
}
