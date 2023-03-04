using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Audio
{
    public string audioName;
    public AudioClip audioClip;
    public List<Loop> Loops;

    public Audio(string _audioname, AudioClip _audioClip)
    {
        audioName = _audioname;
        audioClip = _audioClip;
    }

    public void AddLoop(string _loopname, float x, float y)
    {
        Loops.Add(new Loop(_loopname,x,y));

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