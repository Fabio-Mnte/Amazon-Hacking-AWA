using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Add_questao : MonoBehaviour
{
    public GameObject container;
    private string[] opcoes = new string[50];

    public void ativarPopup()
    {
        //OpenConnection();
        container.transform.parent.parent.gameObject.SetActive(true);
        //CloseConnection();
    }

    public void cancelar()
    {
        container.transform.parent.parent.gameObject.SetActive(false);
    }

    public void salvarQuestao()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject alternativa = container.transform.GetChild(i).gameObject;
            opcoes[i] = (
                alternativa
                    .transform.GetChild(1)
                    .GetChild(0)
                    .GetChild(2)
                    .gameObject.GetComponent<TMP_Text>()
                    .text
            );
            if (alternativa.transform.GetChild(3).gameObject.GetComponent<Toggle>().isOn) {
                Debug.Log("teste sucesso");
             }

            Debug.Log($"{opcoes[i]}");
        }
    }
}
