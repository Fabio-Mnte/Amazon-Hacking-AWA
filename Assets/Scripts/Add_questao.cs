using System;
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
    private int questaoNum = 0;

    // Método para ativar o popup de adição de questão
    public void ativarPopup(int questao)
    {
        string path = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{path}";
        OpenConnection(); // Abre a conexão com o banco de dados
        setQuestao(questao);
        container.transform.parent.parent.gameObject.SetActive(true); // Ativa o popup
        if (questaoNum > 0)
        {
            Debug.Log("Entrou no carregar");
            carregarQuestao();
        }
        else
        {
            container
                .transform.GetChild(4)
                .gameObject.transform.GetChild(1)
                .gameObject.GetComponent<TMP_InputField>()
                .text = "";

            container.transform.GetChild(6).gameObject.SetActive(false);

            for (int i = 0; i < 4; i++)
            {
                container
                    .transform.GetChild(i)
                    .gameObject.transform.GetChild(1)
                    .gameObject.GetComponent<TMP_InputField>()
                    .text = "";

                container
                    .transform.GetChild(i)
                    .gameObject.transform.GetChild(3)
                    .gameObject.GetComponent<Toggle>()
                    .isOn = false;
            }
        }
    }

    private void setQuestao(int questao)
    {
        if(questao != 0){
        int faseId = MainManager.Instance.faseSelected; // Obtém o ID da fase selecionada
        var command = connection.CreateCommand();
        List<int> lista = new List<int>();
        command.CommandText = $"SELECT questao_id FROM questao WHERE fase_id = '{faseId}';";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int teste = Int32.Parse($"{reader["questao_id"]}");
            lista.Add(teste);
        }
        
        Debug.Log(lista[questao - 1]);
        questaoNum = lista[questao - 1];
        Debug.Log("teste");
        } else{
            questaoNum = 0;
        }
    }

    // Método usado para abrir a conexão com o banco de dados
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
        container.transform.parent.parent.parent.GetChild(3).gameObject.SetActive(false); //desativa o popup de deleção
        CloseConnection(); // Fecha a conexão com o banco de dados
    }

    public void carregarQuestao()
    {
        container.transform.GetChild(6).gameObject.SetActive(true);

        int faseId = MainManager.Instance.faseSelected; // Obtém o ID da fase selecionada
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT * FROM questao WHERE fase_id = '{faseId}' AND questao_id = {questaoNum};";
        var reader = command.ExecuteReader();
        reader.Read();
        string pergunta = $"{reader["questao_texto"]}";
        command.CommandText = $"SELECT * FROM opcoes WHERE questao_id = {questaoNum};";
        container
            .transform.GetChild(4)
            .gameObject.transform.GetChild(1)
            .gameObject.GetComponent<TMP_InputField>()
            .text = pergunta;
        int x = 0;
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            opcoes[x] = $"{reader["opcao_texto"]}";
            Debug.Log($"{reader["opcao_texto"]}");
            if ((int)reader["correta"] == 1)
            {
                corretas.Add(x);
            }
            x++;
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{opcoes[i]}");
            container
                .transform.GetChild(i)
                .gameObject.transform.GetChild(1)
                .gameObject.GetComponent<TMP_InputField>()
                .text = $"{opcoes[i]}";
            if (corretas.Contains(i))
            {
                container
                    .transform.GetChild(i)
                    .gameObject.transform.GetChild(3)
                    .gameObject.GetComponent<Toggle>()
                    .isOn = true;
            }
        }
    }

    // Método para salvar a questão e suas opções no banco de dados
    public void salvarQuestao()
    {
        corretas.Clear();
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
        if (questaoNum != 0)
        {
            atualizarQuestao(questao, opcoes, corretas);
            corretas.Clear();
        }
        else
        {
            addBanco(questao, opcoes, corretas); // Adiciona a questão e suas opções ao banco de dados
            corretas.Clear();
        }
        desativarPopup(); // Desativa o popup após salvar a questão
    }

    private void atualizarQuestao(string questao, string[] opcoes, List<int> corretas)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"UPDATE questao SET questao_texto = '{questao}' WHERE questao_id = {questaoNum}";
        command.ExecuteReader();
        for (int i = 0; i < 4; i++)
        {
            if (corretas.Contains(i))
            {
                command.CommandText =
                    $"UPDATE opcoes SET opcao_texto = '{opcoes[i]}', correta = 1 WHERE numero = {i + 1} AND questao_id = {questaoNum}";
                command.ExecuteReader();
            }
            else
            {
                command.CommandText =
                    $"UPDATE opcoes SET opcao_texto = '{opcoes[i]}', correta = 0 WHERE numero = {i + 1} AND questao_id = {questaoNum}";
                command.ExecuteReader();
            }
        }
    }

    public void confirmDelete()
    {
        container.transform.parent.parent.parent.GetChild(3).gameObject.SetActive(true); //desativa o popup de deleção
    }

    public void deletarQuestao()
    {
        if (questaoNum != 0)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM questao WHERE questao_id = {questaoNum}";
            command.ExecuteReader();
            command.CommandText = $"DELETE FROM opcoes WHERE questao_id = {questaoNum}";
            command.ExecuteReader();
        }
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
                $"INSERT INTO opcoes (questao_id, numero, opcao_texto, correta) VALUES ((SELECT questao_id FROM questao WHERE questao_texto = '{questao}'), {i + 1}, '{opcoes[i]}', {CorretaAtual});"; // Insere as opções de resposta na tabela 'opcoes'
            command.ExecuteReader();
        }
    }
}
