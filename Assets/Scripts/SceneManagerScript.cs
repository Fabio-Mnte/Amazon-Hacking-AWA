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
        // Armazene o nome da cena do jogo
        PlayerPrefs.SetString("sceneToLoad", sceneName);

        // Carregue a cena de carregamento
        SceneManager.LoadScene("LoadingScreen");
        
        MainManager.Instance.levelSelected = level;
    }
}
