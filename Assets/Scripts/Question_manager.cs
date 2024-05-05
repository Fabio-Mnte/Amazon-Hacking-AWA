using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;

public class Question_manager : MonoBehaviour
{
    
    private string urlDataBase = "URI=file:Assets/Awa.db";
    private SqliteConnection connection;

    public TMP_Text question;
    public TMP_Text Answr1;
    public TMP_Text Answr2;
    public TMP_Text Answr3;
    public TMP_Text Answr4;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
        //loadData();
        CloseConnection();
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

    public void test_text(string text){
        question.SetText($"{text}");
        
    }

    /*public void loadData(){
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM history as h JOIN fase as f ON h.history_id == f.history_id JOIN questao as q ON q.fase_id == f.fase_id JOIN opcoes as o ON q.questao_id == o.questao_id WHERE h.history_id == {index};";
        var reader = command.ExecuteReader();
        while (reader.Read()){

        }
    } */
    public void get_data(int index)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT historia_texto, fase_texto, questao_texto, opcao_texto FROM historia h LEFT JOIN fase  f ON h.historia_id == f.historia_id LEFT JOIN questao q ON q.fase_id == f.fase_id LEFT JOIN opcoes o ON q.questao_id == o.questao_id WHERE h.historia_id == {index};";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log($"history: {reader["historia_texto"]}, nome: {reader["fase_texto"]}");
        }
    }

    // Update is called once per frame
    void Update() { }
}
