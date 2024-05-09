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
    private string[] opcoes = new string[50]; // Array para armazenar as opções de resposta
    private List<int> corretas = new List<int>(); // Lista para armazenar índices das respostas corretas

    // Método para ativar o popup de adição de questão
    public void ativarPopup()
    {
        string path = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{path}";
        OpenConnection(); // Abre a conexão com o banco de dados
        container.transform.parent.parent.gameObject.SetActive(true); // Ativa o popup
    }

    // Método para abrir a conexão com o banco de dados
    private void OpenConnection()
    {
        Debug.Log($"{urlDataBase}");
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    // Método para fechar a conexão com o banco de dados
    private void CloseConnection()
    {
        connection.Close();
    }

    // Método para desativar o popup de adição de questão
    public void desativarPopup()
    {
        container.transform.parent.parent.gameObject.SetActive(false); // Desativa o popup
        CloseConnection(); // Fecha a conexão com o banco de dados
    }

    // Método para salvar a questão e suas opções no banco de dados
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
                corretas.Add(i); // Adiciona o índice da opção correta à lista
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
        addBanco(questao, opcoes, corretas); // Adiciona a questão e suas opções ao banco de dados
        desativarPopup(); // Desativa o popup após salvar a questão
    }

    // Método para adicionar a questão e suas opções ao banco de dados
    private void addBanco(string questao, string[] opcoes, List<int> corretas)
    {
        int faseId = MainManager.Instance.faseSelected; // Obtém o ID da fase selecionada
        var command = connection.CreateCommand();
        command.CommandText =
            $"INSERT INTO questao (fase_id, questao_texto) VALUES ('{faseId}', '{questao}');"; // Insere a questão na tabela 'questao'
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
                $"INSERT INTO opcoes (questao_id, numero, opcao_texto, correta) VALUES ((SELECT questao_id FROM questao WHERE questao_texto = '{questao}'), {i+1}, '{opcoes[i]}', {CorretaAtual});"; // Insere as opções de resposta na tabela 'opcoes'
            command.ExecuteReader();
        }
    }
}
