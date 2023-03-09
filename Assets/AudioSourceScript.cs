using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public float AudioLenght {get => audioClip.length;}
    [SerializeField] public float AudioTime {get => audioSource.time; set => audioSource.time = value;}
    [SerializeField] public PanelScript owner;
    public bool playLoopOnStart = false;
    public bool playFullOnStart = false;
    private float stopTime;
    private bool StopSound;
    [SerializeField]private Loop currentLoop;
    private bool loopPlaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = audioClip;
        if(playFullOnStart)
            PlayFull();
        if(playLoopOnStart)
            PlayLoop(currentLoop);
    }

    public void PlayLoop(Loop loop)
    {
        StopSound = false;
        stopTime =  AudioLenght;

        loopPlaying = true;
        currentLoop = loop;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(this);

    }

    //Play the full audio
    public void PlayFull()
    {
        StopSound = false;
        loopPlaying = false;
        stopTime =  AudioLenght;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(this);
    }

    //Stops the audio   
    public void Stop()
    {  
        //If the audio is already busy with stopping it stopts it immidiatly
        if(StopSound == true)
        {
            owner.StoppedPlaying();
            AudioManager.Instance.RemoveAsCurrent(this);
        }  

        //If there is no loop playing set the stop time
        if(!loopPlaying)
        {
            stopTime = AudioTime + AudioManager.Instance.FadeValue;
        }

        loopPlaying = false;
        StopSound = true;
    }


    // Update is called once per frame
    void Update()
    {
        //Check if the audio Source is playing
        if(audioSource.isPlaying)
        {   
            //Check if the fade value is not 0 seconds
            if(AudioManager.Instance.FadeValue != 0)
            {
                //Fade in
                if(AudioTime <= AudioManager.Instance.FadeValue)
                {
                    audioSource.volume = (AudioTime/AudioManager.Instance.FadeValue)*AudioManager.Instance.VolumeValue;
                }
                else
                if(stopTime - AudioTime <= AudioManager.Instance.FadeValue)
                {
                    audioSource.volume = ((stopTime - AudioTime)/AudioManager.Instance.FadeValue)*AudioManager.Instance.VolumeValue;

                    if(stopTime - AudioTime <= 0)
                    {
                        audioSource.Stop();
                        owner.StoppedPlaying();
                        AudioManager.Instance.RemoveAsCurrent(this);
                    } 

                }
                else
                {
                    audioSource.volume = AudioManager.Instance.VolumeValue;
                }
                

            }
            else
            {
                audioSource.volume = AudioManager.Instance.VolumeValue;
                if(stopTime - AudioTime <= 0)
                {
                    audioSource.Stop();
                    owner.StoppedPlaying();
                    AudioManager.Instance.RemoveAsCurrent(this);
                }

            }

            if(loopPlaying)
            {
                if(AudioTime >= currentLoop.loopTime.y)
                {
                    AudioTime = currentLoop.loopTime.x;
                    
                }
            }

        }
    }
}
