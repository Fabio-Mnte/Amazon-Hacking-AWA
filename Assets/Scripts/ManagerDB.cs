using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.SqliteClient;

public class ManagerDB : MonoBehaviour
{

    private string urlDataBase = "URI=file:Assets/Awa.db";
    private SqliteConnection connection;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();

        //operações iniciais do banco

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
