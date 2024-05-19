using System.Collections;
using System.Collections.Generic;
using Mono.Data.SqliteClient; // Importa as bibliotecas necessárias para trabalhar com SQLite
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private static string dbPath; // Caminho para o banco de dados
    private string urlDataBase;
    private SqliteConnection connection; // Conexão com o banco de dados
    public GameObject FaseContainer; // Container para as fases ou questões
    private int level; // Nível atual

    void Start()
    {
        dbPath = Application.dataPath + "/Resources/Awa.db";
        urlDataBase = $"URI=file:{dbPath}";
        OpenConnection(); // Abre a conexão com o banco de dados
        Load(); // Carrega os dados dependendo da cena atual
        CloseConnection(); // Fecha a conexão com o banco de dados
    }

    private void Load()
    {
        Scene scene = SceneManager.GetActiveScene(); // Obtém a cena atual
        if (scene.name == "Editar_historia")
        {
            LoadFase(); // Carrega as fases se estiver na cena de edição de história
            return;
        }
        else if (scene.name == "Editar_fase")
        {
            LoadQuest(); // Carrega as questões se estiver na cena de edição de fase
            return;
        }
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM historia";
        var reader = command.ExecuteReader(); // Executa a consulta SQL
        int x = 0;
        while (reader.Read())
        {
            level = (int)reader["historia_id"]; // Obtém o ID da história
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["historia_texto"]}"; // Define o texto da história nas UI Texts
            x++;
        }
        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false; // Desativa os botões restantes
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
            level = (int)reader["fase_id"]; // Obtém o ID da fase
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["fase_texto"]}"; // Define o texto da fase nas UI Texts
            x++;
        }
        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false; // Desativa os botões restantes
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
            level = (int)reader["questao_id"]; // Obtém o ID da questão
            FaseContainer
                .transform.GetChild(x)
                .GetChild(1)
                .gameObject.GetComponent<TMP_Text>()
                .text = $"{reader["questao_texto"]}"; // Define o texto da questão nas UI Texts
            
            
            //LoadButtons(x);
            x++;
        }

        while (x < 6)
        {
            FaseContainer.transform.GetChild(x).gameObject.GetComponent<Button>().interactable =
                false; // Desativa os botões restantes
            x++;
        }
    }

/*
    private void LoadButtons(int x)
    {
        
        FaseContainer
            .transform.GetChild(x)
            .gameObject.GetComponent<Button>()
            .onClick.AddListener(customOnclick);
    }

    private void customOnclick()
    {
        FaseContainer
            .transform.parent.parent.parent.gameObject.GetComponent<Add_questao>()
            .ativarPopup((int) reader["questao_id"]); 
     
        FaseContainer
            .transform.parent.parent.parent.gameObject.GetComponent<Add_questao>()
            .teste("blip"); 
        
    }
    */

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
