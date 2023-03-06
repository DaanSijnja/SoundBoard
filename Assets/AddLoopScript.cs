using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AddLoopScript : MonoBehaviour
{
    //Loop Inputs
    [SerializeField] Slider LoopLowSlider;
    public float LoopLowValue {get => LoopLowSlider.value;}
    [SerializeField] TMP_Text LoopLowText;
    [SerializeField] Slider LoopHighSlider;
    public float LoopHighValue {get => LoopHighSlider.value;}
    [SerializeField] TMP_Text LoopHighText;
    [SerializeField] TMP_InputField LoopName;



    //Buttons
    [SerializeField] Button ConfirmButton;
    [SerializeField] Button CancelButton;
    [SerializeField] Button PlayLoopButton;


    //audioClips
    [SerializeField] public AudioClip selectedAudioClip;

    //Loop 
    public LoopInput loopInput;

    [SerializeField] AudioSource audioSource;




    // Start is called before the first frame update
    void Start()
    {
        //Slider Listeners
        LoopLowSlider.onValueChanged.AddListener(
            (value) =>
            {
                if(selectedAudioClip != null)
                {
                    if(value > LoopHighValue)
                    {
                        LoopLowSlider.value = LoopHighValue;
                    }
                    LoopLowText.text = LoopLowSlider.value * selectedAudioClip.length + "";
                    
                }
                
            }

        );

        LoopHighSlider.onValueChanged.AddListener(
            (value) =>
            {
                if(selectedAudioClip != null)
                {
                    if(value < LoopLowValue)
                    {
                       LoopHighSlider.value = LoopLowValue;
                    }
                    LoopHighText.text = LoopHighSlider.value * selectedAudioClip.length + "";
                }
            }

        );

        audioSource.clip = selectedAudioClip;

        LoopHighText.text = "0";
        LoopLowText.text = "0";

        ConfirmButton.onClick.AddListener(() => Confirm());
        CancelButton.onClick.AddListener(() => Cancel());
        PlayLoopButton.onClick.AddListener(() => PlayLoop());

    }


    void Confirm()
    {
        Loop loop = new Loop("",-1,-1);

        if(LoopLowValue - LoopHighValue != 0)
        {
            if(LoopName.text == "")
                LoopName.text = "Main";
            loop = new Loop(LoopName.text,LoopLowValue*selectedAudioClip.length,LoopHighValue*selectedAudioClip.length);
        }
        
        loopInput.LoopInputConfirm(loop);
   
        Destroy(this.gameObject);
    }

    void Cancel()
    {
        Destroy(this.gameObject);
    }

    void PlayLoop()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
            PlayLoopButton.GetComponentInChildren<TMP_Text>().text = "Play Loop";
        }
        else
        {
            audioSource.Play();
            audioSource.time = LoopLowValue;
            PlayLoopButton.GetComponentInChildren<TMP_Text>().text = "Stop Loop";
        }
    }

    private void Update() 
    {
        if(audioSource.isPlaying)
        {
            if(audioSource.time > LoopHighValue*selectedAudioClip.length)
            {
                audioSource.time = LoopLowValue*selectedAudioClip.length;

            }
        }

    }

}

public interface LoopInput
{
    public void LoopInputConfirm(Loop loop);

}