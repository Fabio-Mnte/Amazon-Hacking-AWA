using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    private int level; // Variável para armazenar o nível selecionado
    
    // Método para carregar uma cena específica
    public void LoadScene(string sceneName)
    { 
        NewLevelSelected(sceneName); // Chama o método para carregar a cena com o nome especificado
    }

    // Método para salvar o nível selecionado
    public void saveScene(int level)
    {
        MainManager.Instance.levelSelected = level; // Salva o nível selecionado no gerenciador principal do jogo
    }

    // Método para salvar a fase selecionada
    public void saveFase(int fase)
    {
        MainManager.Instance.faseSelected = fase; // Salva a fase selecionada no gerenciador principal do jogo
    }

    // Método para carregar uma nova cena
    public void NewLevelSelected(string sceneName)
    {
        // Armazene o nome da cena do jogo a ser carregada
        PlayerPrefs.SetString("sceneToLoad", sceneName);

        // Carregue a cena de carregamento
        SceneManager.LoadScene("LoadingScreen");
        
        
    }
}
