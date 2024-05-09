using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Add_questao : MonoBehaviour
{
    public GameObject container;
    private string urlDataBase;
    private SqliteConnection connection;
    private string[] opcoes = new string[50];
    private List<int> corretas = new List<int>();

    public void ativarPopup()
    {
        string path = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{path}";
        OpenConnection();
        container.transform.parent.parent.gameObject.SetActive(true);
        
    }

    private void OpenConnection()
    {
        Debug.Log($"{urlDataBase}");
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    private void CloseConnection()
    {
        connection.Close();
    }

    public void desativarPopup()
    {
        container.transform.parent.parent.gameObject.SetActive(false);
        CloseConnection();
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
            if (alternativa.transform.GetChild(3).gameObject.GetComponent<Toggle>().isOn)
            {
                corretas.Add(i);
            }

            Debug.Log($"{opcoes[i]}");
        }
        string questao = container
            .transform.GetChild(4)
            .GetChild(1)
            .GetChild(0)
            .GetChild(2)
            .gameObject.GetComponent<TMP_Text>()
            .text;
        addBanco(questao, opcoes, corretas);
        desativarPopup();
    }

    private void addBanco(string questao, string[] opcoes, List<int> corretas)
    {
        int faseId = MainManager.Instance.faseSelected;
        var command = connection.CreateCommand();
        command.CommandText =
            $"INSERT INTO questao (fase_id, questao_texto) VALUES ('{faseId}', '{questao}');";
        command.ExecuteReader();
        int CorretaAtual;
        for (int i = 0; i < 4; i++)
        {
            if (corretas.Contains(i))
            {
                CorretaAtual = 1;
            }
            else
            {
                CorretaAtual = 0;
            }
            command.CommandText =
                $"INSERT INTO opcoes (questao_id, numero, opcao_texto, correta) VALUES ((SELECT questao_id FROM questao WHERE questao_texto = '{questao}'), {i+1}, '{opcoes[i]}', {CorretaAtual});";
            command.ExecuteReader();
        }
        
    }
}
