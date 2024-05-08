using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private static string dbPath;
    private string urlDataBase;
    private SqliteConnection connection;
    public GameObject FaseContainer;
    private int level;

    void Start()
    {
        dbPath = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{dbPath}";
        //Debug.Log($"{urlDataBase}");
        OpenConnection();
        Load();
        CloseConnection();
    }

    private void Load()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Editar_historia")
        {
            Debug.Log($"Funcionando");
            LoadFase();
            return;
        }
        else if(scene.name == "Editar_fase"){
            LoadQuest();
            return;
        }
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM historia";
        var reader = command.ExecuteReader();
        int x = 0;
        while (reader.Read())
        {
            level = (int)reader["historia_id"];
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["historia_texto"]}";
            x++;
        }
        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false;
            x++;
        }
    }

    private void LoadFase()
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT fase_id, fase_texto FROM historia h JOIN fase f ON h.historia_id == f.historia_id";
        var reader = command.ExecuteReader();
        int x = 0;
        while (reader.Read())
        {
            level = (int)reader["fase_id"];
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["fase_texto"]}";
            x++;
        }
        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false;
            x++;
        }
    }
    private void LoadQuest()
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT questao_id, questao_texto FROM historia h JOIN fase f ON h.historia_id == f.historia_id JOIN questao q ON f.fase_id = q.fase_id;";
        var reader = command.ExecuteReader();
        int x = 0;
        while (reader.Read())
        {
            level = (int)reader["questao_id"];
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["questao_texto"]}";
            x++;
        }
        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false;
            x++;
        }
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

    // Update is called once per frame
    void Update() { }
}
