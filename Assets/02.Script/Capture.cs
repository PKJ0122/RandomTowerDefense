using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture texture;
    public Image bg;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void CaptureButton()
    {
        StartCoroutine(C_CaptureImage());
    }

    IEnumerator C_CaptureImage()
    {
        yield return null;

        Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = texture;
        tex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);

        yield return null;

        var data = tex.EncodeToPNG();
        string name = Random.Range(0,int.MaxValue).ToString();
        string ex = ".png";
        string path = Application.persistentDataPath + "/Thumbnail/";

        Debug.Log(path);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path+name+ex, data);

        yield return null;
    }
}
