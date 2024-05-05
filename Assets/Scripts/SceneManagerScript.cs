using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    private int level;
    public void LoadScene(string sceneName)
    { 
        NewLevelSelected(level);
        SceneManager.LoadScene("Game_1");
    }

    public void NewLevelSelected(int level)
    {
        MainManager.Instance.levelSelected = level;
    }
}
