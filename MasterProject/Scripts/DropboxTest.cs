using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DropboxTest : MonoBehaviour
{
    public string filePath = "Assets/myfile.csv";

    private void Start()
    {
        if (string.IsNullOrEmpty(this.filePath) || !File.Exists(this.filePath))
        {
            Debug.LogError(this.ToString() + " No 'myfile.csv' file found");
            this.enabled = false;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(5f, 5f, 100f, 50f), "Upload File"))
        {
            UploadFileToDropbox(this.filePath);
        }
    }

    public void UploadFileToDropbox(string path)
    {
        var text = File.ReadAllText(path);
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException("path", path + " is not a valid path or no file exists at the path");
        }

        var bytes = Encoding.UTF8.GetBytes(text);
        if (bytes.Length == 0)
        {
            throw new ArgumentNullException("bytes", "File at " + path + " did not give a valid bytes length after encoding");
        }
        
        var data = @"
        {
            'path': '" + path + @"',
            'mode': 'add',
            'autorename': true,
            'mute': false
        }".Trim();

        var headers = new Dictionary<string, string>(3);
        headers.Add("Authorization", "Bearer ie9epjU43NcAAAAAAAATeutD8nT5PHuUU3hL7NM2QDTzZNKUpIzdUKLfF2TDErr5");
        headers.Add("Content-Type", "application/octet-stream");
        headers.Add("Dropbox-API-Arg", data);

        var form = new WWWForm();
        form.AddBinaryData("data-binary", bytes);

        var www = new WWW("https://content.dropboxapi.com/2/files/upload", form.data, headers);
        Debug.Log(this.ToString() + " making a POST to :" + www.url);

        StartCoroutine(UploadFileToDropboxInternal(www));
    }

    private IEnumerator UploadFileToDropboxInternal(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW Success: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error + "\n" + www.text);
        }
    }
}