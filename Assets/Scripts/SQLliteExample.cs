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
        InsertData("Arthur");
        InsertData("Leonardo");
        PrintData();
        DeleteData(1);
    }

    private void OpenConnection()
    {
        connection = new SqliteConnection(urlDataBase);
        connection.Open();
    }

    private void CreateTable()
    {
        var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS MyTable (ID INTEGER PRIMARY KEY, Name TEXT)";
        command.ExecuteNonQuery();
    }

    public void InsertData(string name)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"INSERT INTO MyTable (Name) VALUES ('{name}')";
        command.ExecuteNonQuery();
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
