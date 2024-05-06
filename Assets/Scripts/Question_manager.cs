using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question_manager : MonoBehaviour
{
    private static string path;
    private int questaoAtual = 0;
    private string urlDataBase;
    private SqliteConnection connection;
    public GameObject OptionContainer;
    public TMP_Text question;
    public TMP_Text resultado;

    public TMP_Text placar;
    private int time1;
    private int time2;

    private int limite;

    private int index = MainManager.Instance.levelSelected;
    void Start()
    {
         
        path = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{path}";
        Debug.Log($"{path} ");
        setLimite();
        loadData();
    }

    private void setLimite()
    {
        OpenConnection();
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT count(q.questao_id) as limite FROM historia as h JOIN fase as f ON h.historia_id == f.historia_id JOIN questao as q ON q.fase_id == f.fase_id WHERE h.historia_id == {index};";
        var reader = command.ExecuteReader();
        reader.Read();
        limite = int.Parse($"{reader["limite"]}");
        Debug.Log($"{limite}");
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

            sceneManager.NewLevelSelected("Menu_alunos");
        }
        else
        {
            loadData();
        }
    }

    public void loadData()
    {
        destravar();
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
            question.text = $"{reader["questao_texto"]}";
            OptionContainer
                .transform.GetChild(i)
                .GetChild(0)
                .GetChild(0)
                .GetChild(0)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["opcao_texto"]}";
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
            button.interactable = false;
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
            button.interactable = true;
        }
    }

    public void escolherResultado(int escolha)
    {
        Debug.Log($"{escolha}");
        OpenConnection();
        travar();
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
        }
        else
        {
            resultado.text = "Ninguém acertou!";
        }
        resultado.GetComponent<TMP_Text>().enabled = true;
        Button botao = resultado.transform.parent.GetChild(5).GetComponent<Button>();
        botao.gameObject.SetActive(true);
        Debug.Log($"nomebotao: {resultado.transform.parent.GetChild(5).name}");
        CloseConnection();
    }
}
