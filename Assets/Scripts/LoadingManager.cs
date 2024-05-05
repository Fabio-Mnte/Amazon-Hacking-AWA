using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider;

    void Start()
    {
        // Obtenha o nome da cena do jogo
        string sceneName = PlayerPrefs.GetString("sceneToLoad");

        StartCoroutine(LoadGameScene(sceneName));
    }

    IEnumerator LoadGameScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            if (loadingSlider != null)
            {
                loadingSlider.value = asyncOperation.progress;
            }
            yield return null;
        }

    }
}
