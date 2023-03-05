using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using UnityEngine.Events;

public class FolderBrowser : MonoBehaviour
{
    public bool busy = false;
    public AudioClip LastAudioClipFromPath;
    public static FolderBrowser Instance;

    public UnityAction<AudioClip> foundClip;

    private void Start() {

        if(Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;


    }


    public string OpenFileBrowser()
    {
        string path = "";

        var bp = new BrowserProperties();
        bp.filter = "Audio File (*.mp3) | *mp3";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, foundPath => {
            path = foundPath;
        });

        return path;
    }

    public void OpenFileFromPath(string path, ref AudioClip selectedClip)
    {
        busy = true;
        StartCoroutine(LoadAudioClip(path));
        float StartTime = Time.time;
    
    }

    IEnumerator LoadAudioClip(string path)
    {   
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path,AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                LastAudioClipFromPath = null;
                foundClip.Invoke(null);
                busy = false;
            }
            else
            {
                LastAudioClipFromPath = DownloadHandlerAudioClip.GetContent(uwr);
                foundClip.Invoke(LastAudioClipFromPath);
                Debug.Log(LastAudioClipFromPath);
                busy = false;
            }

        }

    }



}   

