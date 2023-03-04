using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{



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
        if(Instance != null)
            Destroy(this.gameObject);

        Instance = this;

        StopSoundButton.onClick.AddListener(
            () =>
            {
                if(currentCombatMusic != null)
                {
                    currentCombatMusic.Stop();
                }

            }
        );


    }

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

            case "BackgroundMusic":
                if(currentBackgroundMusic != null)
                {
                    currentBackgroundMusic.Stop();
                }

                currentBackgroundMusic = pannel;

            break;

        }

    }

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

            case "BackgroundMusic":
                if(currentBackgroundMusic == pannel)
                {
                    currentBackgroundMusic = null;
                }

            break;

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    

}
