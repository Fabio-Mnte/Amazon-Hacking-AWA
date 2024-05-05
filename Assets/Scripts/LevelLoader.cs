using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private string urlDataBase = "URI=file:Assets/Awa.db";

    private SqliteConnection connection;
    //public List<TMP_Text> levelTextList;
    public GameObject FaseContainer;
    //public TMP_Text LevelText;
    //public Button button;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
        Load();
        CloseConnection();
    }

    private void Load()
    {
        
        Debug.Log($"{FaseContainer.transform.GetChild(2).name}");
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM historia";
        var reader = command.ExecuteReader();
        int i = 0;
        while (reader.Read())
        {
            level = (int)reader["historia_id"];
            //levelTextList[i].text = $"{reader["historia_texto"]}";
            FaseContainer.transform.GetChild(i).GetChild(1).gameObject.GetComponent<TMP_Text>().text= $"{reader["historia_texto"]}";
            //Debug.Log($"historia: {reader["historia_texto"]}, id: {reader["historia_id"]}");
            //Debug.Log($"{levelTextList[i].transform.parent.name}");
            //levelTextList[i].transform.parent.gameObject.GetComponent<Button>().interactable = false;
            //button.interactable = false;
            i++;
        }
        for(i = i; i < 6; i++){
            //levelTextList[i].transform.parent.gameObject.GetComponent<Button>().interactable = false;
            FaseContainer.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
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
