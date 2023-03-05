using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class PanelScript : MonoBehaviour
{   
    //Grid position
    [SerializeField] public Vector2 gridPos;
    //the Audio class
    [SerializeField] public Audio audioItem;
    //the audio group this pannel belongs to
    [SerializeField] public string audioGroup;

    //Text Mesh Pro Text for the title
    [SerializeField] TMP_Text nameAudio;
    //End button
    [SerializeField] Button EndButton;
    //Play Button
    [SerializeField] Button PlayButton;
    //Button prefab for the loops
    [SerializeField] GameObject LoopButtonPrefab;

    //The Audio source
    [SerializeField] AudioSource audioSource;
    //If the sound is stopping
    [SerializeField] public bool StopSound = false;

    //loop relateded vars
    private float stopTime;
    private bool loopPlaying;
    private Loop currentLoop;


    //Easy acces vars
    float AudioTime {get => audioSource.time; set => audioSource.time = value;}
    float AudioLenght {get => audioItem.GetAudioClip().length;}
    



    // Start is called before the first frame update
    void Start()
    {   
        //Check if there is a audioItem
        if(audioItem != null)
        {
            //Set the text to the name of the audio item
            nameAudio.text = audioItem.audioName;
            OpenAudioFromPath(audioItem.audioPath);
            //Loop for each Loop in the audio item
            int i = 0;
            foreach(Loop loop in audioItem.Loops)
            {   
                //Max 3 loops on display
                if(i >= 3)
                    break;

                //Instanciate the new Loop button and set some values
                var obj = Instantiate(LoopButtonPrefab);
                obj.GetComponentInChildren<TMP_Text>().text = loop.loopName;
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(0,-47.9f + i*30,0);

                //Add an addlistener for a button press
                obj.GetComponent<Button>().onClick.AddListener( () => PlayLoop(loop) );

                i++;
            }

        }

        //Set the AudioSource Clip to the AudioItem audioclip
        

        //Add Listeners to the Play and End Button
        PlayButton.onClick.AddListener( () => PlayFull() );
        EndButton.onClick.AddListener( () => Stop() );


    }

    //Play the audio normal and adds sets the Loop
    public void PlayLoop(Loop loop)
    {
        StopSound = false;
        stopTime =  AudioLenght;

        loopPlaying = true;
        currentLoop = loop;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(audioGroup,this);

    }

    //Play the full audio
    public void PlayFull()
    {
        StopSound = false;
        loopPlaying = false;
        stopTime =  AudioLenght;
        audioSource.Play();
        AudioManager.Instance.SetAsCurrent(audioGroup,this);
    }

    //Stops the audio   
    public void Stop()
    {  
        //If the audio is already busy with stopping it stopts it immidiatly
        if(StopSound == true)
        {
            audioSource.Stop();
            AudioManager.Instance.RemoveAsCurrent(audioGroup,this);
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

    //File Opening
    public void OpenAudioFromPath(string path)
    {
     
        StartCoroutine(LoadAudioClip(path));
    
    }

    IEnumerator LoadAudioClip(string path)
    {   
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path,AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                
                
            }
            else
            {
                audioItem.SetAudioClip(DownloadHandlerAudioClip.GetContent(uwr));
                audioSource.clip = audioItem.GetAudioClip();
                

            }

        }
    }

}
