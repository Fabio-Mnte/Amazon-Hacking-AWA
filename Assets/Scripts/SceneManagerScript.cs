using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    private int level;

    public void ExitGame() {
        Debug.Log("Teste");
        Application.Quit(); //Ignorado no Editor.
    }
    
    public void LoadScene(string sceneName)
    { 
        NewLevelSelected(sceneName);
        //SceneManager.LoadScene("Game_1");
    }
    public void saveScene(int level){
        MainManager.Instance.levelSelected = level;
    }

    public void NewLevelSelected(string sceneName)
    {
        // Armazene o nome da cena do jogo
        PlayerPrefs.SetString("sceneToLoad", sceneName);

        // Carregue a cena de carregamento
        SceneManager.LoadScene("LoadingScreen");
        
        //MainManager.Instance.levelSelected = level;
    }
}
