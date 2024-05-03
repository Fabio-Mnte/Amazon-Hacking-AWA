using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;

public class Question_manager : MonoBehaviour
{
    private string urlDataBase = "URI=file:MasterSQLite.db";
    private SqliteConnection connection;

    private TextMeshProUGUI question_text;
    private TextMeshProUGUI Answr1_text;
    private TextMeshProUGUI Answr2_text;
    private TextMeshProUGUI Answr3_text;
    private TextMeshProUGUI Answr4_text;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
    }

    private void OpenConnection()
    {
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    private void get_data(int index)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT * FROM history as h JOIN fase as f ON h.history_id == f.history_id JOIN questao as q ON q.fase_id == f.fase_id JOIN opcoes as o ON q.questao_id == o.questao_id WHERE h.history_id == {index};";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log($"ID: {reader["history_id"]}, Name: {reader["nome"]}");
        }
    }

    // Update is called once per frame
    void Update() { }
}
