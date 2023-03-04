using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelScript : MonoBehaviour
{
    [SerializeField] public Vector2 gridPos;
    [SerializeField] public Audio audioItem;
    [SerializeField] public string audioGroup;
    [SerializeField] TMP_Text nameAudio;
    [SerializeField] Button EndButton;
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject LoopButtonPrefab;
    [SerializeField] AudioSource audioSource;

    [SerializeField] public bool StopSound = false;
    private float stopTime;
    private bool loopPlaying;
    private Loop currentLoop;

    float AudioTime {get => audioSource.time; set => audioSource.time = value;}
    float AudioLenght {get => audioItem.audioClip.length;}
    



    // Start is called before the first frame update
    void Start()
    {
        if(audioItem != null)
        {
            nameAudio.text = audioItem.audioName;

            int i = 0;
            foreach(Loop loop in audioItem.Loops)
            {
                if(i >= 3)
                    break;
                var obj = Instantiate(LoopButtonPrefab);
                obj.GetComponentInChildren<TMP_Text>().text = loop.loopName;
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(0,-47.9f + i*30,0);

                obj.GetComponent<Button>().onClick.AddListener( () => PlayLoop(loop) );

                i++;
            }

        }

        audioSource.clip = audioItem.audioClip;

        PlayButton.onClick.AddListener( () => PlayFull() );
        EndButton.onClick.AddListener( () => Stop() );


    }


    public void PlayLoop(Loop loop)
    {
        StopSound = false;
        stopTime =  AudioLenght;

        loopPlaying = true;
        currentLoop = loop;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(audioGroup,this);

    }

    public void PlayFull()
    {
        StopSound = false;
        loopPlaying = false;
        stopTime =  AudioLenght;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(audioGroup,this);
    }

    public void Stop()
    {  
        if(StopSound == true)
        {
            audioSource.Stop();
            AudioManager.Instance.RemoveAsCurrent(audioGroup,this);
        }
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
        if(audioSource.isPlaying)
        {   
            if(AudioManager.Instance.FadeValue != 0)
            {
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
                        AudioManager.Instance.RemoveAsCurrent(audioGroup,this);
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
                    AudioManager.Instance.RemoveAsCurrent(audioGroup,this);
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
