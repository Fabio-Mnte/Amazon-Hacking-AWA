using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question_manager : MonoBehaviour
{
    private static string path; // Caminho para o banco de dados
    private int questaoAtual = 0; // Índice da questão atual
    private string urlDataBase; // URL para a conexão com o banco de dados
    private SqliteConnection connection; // Conexão com o banco de dados
    public GameObject OptionContainer; // Container para as opções de resposta
    public TMP_Text question; // Texto da pergunta
    public TMP_Text resultado; // Texto do resultado da resposta
    public TMP_Text placar; // Texto do placar
    private int time1; // Pontuação do Time 1
    private int time2; // Pontuação do Time 2
    private int limite; // Número máximo de questões
    private int index = MainManager.Instance.levelSelected; // Índice do nível selecionado

    void Start()
    {
        path = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{path}";
        setLimite(); // Define o limite de questões
        loadData(); // Carrega a primeira questão
    }

    private void setLimite()
    {
        OpenConnection();
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT count(q.questao_id) as limite FROM historia as h JOIN fase as f ON h.historia_id == f.historia_id JOIN questao as q ON q.fase_id == f.fase_id WHERE h.historia_id == {index};";
        var reader = command.ExecuteReader();
        reader.Read();
        limite = int.Parse($"{reader["limite"]}"); // Obtém o número máximo de questões
    }

    private void OpenConnection()
    {
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    private void CloseConnection()
    {
        connection.Close();
    }

    public void test_text(string text)
    {
        question.SetText($"{text}");
    }

    public void check_final()
    {
        if (questaoAtual == limite)
        {
            SceneManagerScript sceneManager = FindObjectOfType<SceneManagerScript>();
            sceneManager.NewLevelSelected("Menu_alunos"); // Carrega a cena do menu de alunos após completar todas as questões
        }
        else
        {
            loadData(); // Carrega a próxima questão
        }
    }

    public void loadData()
    {
        destravar(); // Habilita os botões das opções de resposta
        OpenConnection();
        Button botao = resultado.transform.parent.GetChild(5).GetComponent<Button>();
        botao.gameObject.SetActive(false);
        resultado.GetComponent<TMP_Text>().enabled = false;
        questaoAtual++;
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT * FROM historia as h JOIN fase as f ON h.historia_id == f.historia_id JOIN questao as q ON q.fase_id == f.fase_id JOIN opcoes as o ON q.questao_id == o.questao_id WHERE h.historia_id == {index} AND q.questao_id == {questaoAtual};";
        var reader = command.ExecuteReader();
        int i = 0;
        while (reader.Read())
        {
            question.text = $"{reader["questao_texto"]}"; // Define o texto da pergunta
            OptionContainer
                .transform.GetChild(i)
                .GetChild(0)
                .GetChild(0)
                .GetChild(0)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["opcao_texto"]}"; // Define o texto das opções de resposta
            i++;
        }
        CloseConnection();
    }

    private void travar()
    {
        for (int y = 0; y < 4; y++)
        {
            Button button = OptionContainer
                .transform.GetChild(y)
                .GetChild(0)
                .GetChild(0)
                .gameObject.GetComponent<Button>();
            button.interactable = false; // Desabilita os botões das opções de resposta
        }
    }

    private void destravar()
    {
        for (int y = 0; y < 4; y++)
        {
            Button button = OptionContainer
                .transform.GetChild(y)
                .GetChild(0)
                .GetChild(0)
                .gameObject.GetComponent<Button>();
            button.interactable = true; // Habilita os botões das opções de resposta
        }
    }

    public void escolherResultado(int escolha)
    {
        Debug.Log($"{escolha}");
        OpenConnection();
        travar(); // Desabilita os botões das opções de resposta
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT o.numero FROM historia as h JOIN fase as f ON h.historia_id == f.historia_id JOIN questao as q ON q.fase_id == f.fase_id JOIN opcoes as o ON q.questao_id == o.questao_id WHERE h.historia_id == {index} AND q.questao_id == {questaoAtual} AND correta == 1;";
        var reader = command.ExecuteReader();
        reader.Read();
        int numValue = int.Parse($"{reader["numero"]}");
        if (escolha == numValue)
        {
            resultado.text = "Jogador 1 acertou!";
            time1++;
            placar.text = $"time 1 \n{time1} pontos\ntime 2 \n{time2} pontos";
            AudioManager.Instance.PlaySFX("AcertarQuestao");
        }
        else
        {
            resultado.text = "Ninguém acertou!";
            AudioManager.Instance.PlaySFX("ErrarQuestao");
        }
        resultado.GetComponent<TMP_Text>().enabled = true; // Habilita a exibição do resultado
        Button botao = resultado.transform.parent.GetChild(5).GetComponent<Button>();
        botao.gameObject.SetActive(true); // Ativa o botão para avançar para a próxima questão
        CloseConnection();
    }
}
