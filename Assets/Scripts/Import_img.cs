using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenImagesplz : MonoBehaviour
{
    string path;
    public RawImage image;

    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        GetImage();
    }

    void GetImage()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError) 
            Debug.Log(request.error);
        else
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
