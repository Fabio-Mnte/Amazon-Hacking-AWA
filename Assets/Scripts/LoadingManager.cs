using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider; // Referência ao slider para exibir o progresso do carregamento

    void Start()
    {
        // Obtenha o nome da cena do jogo a ser carregada
        string sceneName = PlayerPrefs.GetString("sceneToLoad");

        StartCoroutine(LoadGameScene(sceneName)); // Inicia o carregamento da cena do jogo de forma assíncrona
    }

    IEnumerator LoadGameScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName); // Carrega a cena de forma assíncrona
        asyncOperation.allowSceneActivation = false; // Impede que a cena seja ativada automaticamente após o carregamento completo

        float progress = 0f;
        while (progress < 1f)
        {
            // Calcula o progresso do carregamento de forma suave
            progress = Mathf.Lerp(
                progress,
                asyncOperation.progress < 0.9f ? asyncOperation.progress : 1f, // Limita o progresso a 0.9 para evitar que o Lerp ultrapasse 1
                Time.deltaTime * 5 // Taxa de suavização
            );
            if (loadingSlider != null)
            {
                loadingSlider.value = progress; // Atualiza o valor do slider com o progresso do carregamento
            }

            // Se o progresso estiver completo e a cena ainda não estiver ativada, ative a cena
            if (progress >= 0.999f && !asyncOperation.allowSceneActivation)
            {
                asyncOperation.allowSceneActivation = true; // Ativa a cena
            }

            yield return null; // Aguarda um frame antes de continuar
        }
    }
}
