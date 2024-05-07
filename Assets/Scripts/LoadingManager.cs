using System;
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
        asyncOperation.allowSceneActivation = false;

        float progress = 0f;
        while (progress < 1f)
        {
            progress = Mathf.Lerp(
                progress,
                asyncOperation.progress < 0.9f ? asyncOperation.progress : 1f,
                Time.deltaTime * 5
            );
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            // Se o progresso estiver completo e a cena ainda n�o estiver ativada, ative a cena
            if (progress >= 0.999f && !asyncOperation.allowSceneActivation)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
