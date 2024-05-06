using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.SqliteClient;

public class ManagerDB : MonoBehaviour
{
    private static string path;
    private string urlDataBase = $"URI=file:{path}";
    private SqliteConnection connection;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/Resources/Awa.db";
        OpenConnection();

        //opera��es iniciais do banco

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
}
