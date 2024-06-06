using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcertouScript : MonoBehaviour
{

    public TMP_Text resultado;

    // Start is called before the first frame update
    void Start()
    {
        resultado = GameObject.Find("Resultado").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckTry(int questionID)
    {
        if(questionID == 1)
        {
            resultado.text = "Jogador 1 acertou!";
        }
        else
        {
            resultado.text = "Ninguém acertou!";
        }
        resultado.GetComponent<TMP_Text>().enabled = true; // Habilita a exibição do resultado
    }
}
