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

    [SerializeField] public PanelScript currentBackgroundMusic;
    [SerializeField] public PanelScript currentCombatMusic;


    public static AudioManager Instance;
    // Start is called before the first frame update
    void Start()
    {   
        //Set Instance to this so it can be revrenced every where
        if(Instance != null)
            Destroy(this.gameObject);

        Instance = this;


        //Call when the StopSound button is pressed
        StopSoundButton.onClick.AddListener(
            () =>
            {   
                //Stop the current music
                if(currentCombatMusic != null)
                {
                    currentCombatMusic.Stop();
                }

            }
        );


    }

    //Set a pannel as current playing pannel
    public void SetAsCurrent(string audioGroup, PanelScript pannel)
    {
        switch(audioGroup)
        {
            case "CombatMusic":
                if(currentCombatMusic != null)
                {
                    currentCombatMusic.Stop();
                }

                currentCombatMusic = pannel;

            break;

        
        }

    }

    //Remove as current playing pannel
    public void RemoveAsCurrent(string audioGroup, PanelScript pannel)
    {
        switch(audioGroup)
        {
            case "CombatMusic":
                if(currentCombatMusic == pannel)
                {
                    currentCombatMusic = null;
                }


            break;

        
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    

}
