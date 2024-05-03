using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.SqliteClient;

public class SQLiteManager : MonoBehaviour
{
    
    private string urlDataBase = "URI=file:MasterSQLite.db";
    private SqliteConnection connection;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
        CreateTable();
        //InsertData("Arthur");
        //InsertData("Leonardo");
        //PrintData();
        //DeleteData(1);
        
    }

    private void OpenConnection()
    {
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    private void CreateTable()
    {
       
        var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS history (history_id INTEGER  PRIMARY KEY, nome VARCHAR); CREATE TABLE IF NOT EXISTS fase (fase_id INTEGER  PRIMARY KEY ,history_id INT,nome VARCHAR,FOREIGN KEY (history_id) REFERENCES history(history_id)); CREATE TABLE IF NOT EXISTS questao (questao_id INTEGER  PRIMARY KEY , fase_id INT, FOREIGN KEY (fase_id) REFERENCES fase(fase_id)); CREATE TABLE IF NOT EXISTS opcoes (opcoes_id INTEGER  PRIMARY KEY , questao_id INT, FOREIGN KEY (questao_id) REFERENCES questao(questao_id));";
    
        command.ExecuteNonQuery();
        
    }

    public void delete_database(){
        var command = connection.CreateCommand();
        command.CommandText = "DROP TABLE history; DROP TABLE fase; DROP TABLE questao; DROP TABLE opcoes;";
        command.ExecuteNonQuery();
    }

    public void InsertData(string name)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"INSERT INTO history (nome) VALUES ('{name}')";
        command.ExecuteNonQuery();
    }
    public void drop_tables(){

    }
    public void insert_test(){
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM history;";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log($"ID: {reader["history_id"]}, Name: {reader["nome"]}");
        }

        }
    
    public void PrintData()
    {
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM MyTable";
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log($"ID: {reader["ID"]}, Name: {reader["Name"]}");
        }

    }

    public void DeleteData(int id)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM MyTable WHERE ID = {id}";
        command.ExecuteNonQuery();
        
    }
    

}
