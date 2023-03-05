using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//The class for a Audio with a name and Loops
[System.Serializable]
public class Audio
{
    public string audioName;
    public AudioClip audioClip;
    private string audioPath;
    public List<Loop> Loops;

    public Audio(string _audioname, AudioClip _audioClip)
    {
        audioName = _audioname;
        audioClip = _audioClip;
    }

    public void SetAudioPath(string path)
    {
        audioPath = path;
    }

    public void AddLoop(string _loopname, float x, float y)
    {
        Loops.Add(new Loop(_loopname,x,y));

    }

    public void TOJSON()
    {


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