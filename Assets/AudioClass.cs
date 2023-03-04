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


}


[System.Serializable]
public struct Loop
{
    public string loopName;
    public Vector2 loopTime;

}