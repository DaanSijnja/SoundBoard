using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//The class for a Audio with a name and Loops
[System.Serializable]
public class Audio
{
    public string audioName;
    private AudioClip audioClip;
    public string audioPath;
    public List<Loop> Loops;

    public Audio()
    {
        Loops = new List<Loop>();
    }

    public void SetAudioClip(AudioClip _audio)
    {
        audioClip = _audio;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public void AddLoop(string _loopname, float x, float y)
    {   
        Loop loop = new Loop(_loopname,x,y);
        Loops.Add(loop);

    }

    public string TOJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public void FROMJSON(string JSON)
    {
        var loadedJSON = JsonUtility.FromJson<Audio>(JSON); 
        audioName = loadedJSON.audioName;
        audioPath = loadedJSON.audioPath;
        foreach(Loop loop in loadedJSON.Loops)
        {
            Loops.Add(loop);
        }
    }
}


[System.Serializable]
public struct Loop
{
    public string loopName;
    public Vector2 loopTime;

    public Loop(string _name, float x, float y)
    {   
        loopName = _name;
        loopTime = new Vector2(x,y);
    }
}