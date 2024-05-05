using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Armazene o nome da cena do jogo
        PlayerPrefs.SetString("sceneToLoad", sceneName);

        // Carregue a cena de carregamento
        SceneManager.LoadScene("LoadingScreen");
    }
}
