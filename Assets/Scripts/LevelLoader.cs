using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
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
        Debug.Log($"{urlDataBase}");
        OpenConnection();
        Load();
        CloseConnection();
    }

    private void Load()
    {
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
        while(x < 6)
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
