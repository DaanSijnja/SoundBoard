using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{


    //Sliders and button for Fading of sound, the volume and a StopButton
    [SerializeField] Slider fadeInSlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Button StopSoundButton;

    public float FadeValue {get => fadeInSlider.value; }
    public float VolumeValue {get => volumeSlider.value; }

    public List<AudioSourceScript> currentlyPlayingAudios;


    public static AudioManager Instance;
    // Start is called before the first frame update
    void Start()
    {   
        //Set Instance to this so it can be revrenced every where
        if(Instance != null)
            Destroy(this.gameObject);

        Instance = this;

        currentlyPlayingAudios = new List<AudioSourceScript>();
        //Call when the StopSound button is pressed
        StopSoundButton.onClick.AddListener(
            () =>
            {   
              
               
            }
        );


    }

    //Set a pannel as current playing pannel
    public void SetAsCurrent(AudioSourceScript audioSource)
    {
        if(!currentlyPlayingAudios.Contains(audioSource))
        {
            currentlyPlayingAudios.Add(audioSource);
        }

    }

    //Remove as current playing pannel
    public void RemoveAsCurrent(AudioSourceScript audioSource)
    {
        if(currentlyPlayingAudios.Contains(audioSource))
        {
            currentlyPlayingAudios.Remove(audioSource);
            Destroy(audioSource.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    

}
