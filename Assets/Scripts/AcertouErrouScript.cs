using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcertouScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckTry(int questionID)
    {
        if(questionID == 1)
        {
            Debug.Log("certa");
        }
        else
        {
            Debug.Log("Errada");
        }
    }
}
