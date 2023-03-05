using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using TMPro;

public class NewPannelInput : MonoBehaviour
{

    [SerializeField] TMP_InputField audioName;
    //Browse Inputs
    [SerializeField] TMP_InputField BrowseField;
    [SerializeField] Button BrowseButton;

    //Loop Inputs
    [SerializeField] Slider LoopLowSlider;
    public float LoopLowValue {get => LoopLowSlider.value;}
    [SerializeField] TMP_Text LoopLowText;
    [SerializeField] Slider LoopHighSlider;
    public float LoopHighValue {get => LoopHighSlider.value;}
    [SerializeField] TMP_Text LoopHighText;
    [SerializeField] TMP_InputField LoopName;
    [SerializeField] Slider PlayTimeSlider;

    //Buttons
    [SerializeField] Button PlayButton;
    [SerializeField] Button PlayLoopButton;
    [SerializeField] Button ConfirmButton;
    [SerializeField] Button CancelButton;

    [SerializeField] AudioSource audioSource;

    public AudioClip selectedAudioClip;
    private string selectedAudioPath;
    
  

    public AddPannelScript ownerAddPannel;

    // Start is called before the first frame update
    void Start()
    {
        
        
        //Onclicks for the buttons
        ConfirmButton.onClick.AddListener(() => Confirm());
        BrowseButton.onClick.AddListener(() =>  Browse());
        CancelButton.onClick.AddListener(() => Cancel());
        PlayButton.onClick.AddListener(() => Play());
        PlayLoopButton.onClick.AddListener(() => PlayLoop());


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

        LoopHighText.text = "0";
        LoopLowText.text = "0";

    }

    //Gets all the values and makes a new Audio Class object
    void Confirm()
    {
        var audio = new Audio();
        audio.audioName = audioName.text;
        audio.SetAudioClip(selectedAudioClip);

        audio.audioPath = selectedAudioPath;

        if(LoopLowValue - LoopHighValue != 0)
        {
            if(LoopName.text == "")
                LoopName.text = "Main";
            audio.AddLoop(LoopName.text,LoopLowValue*selectedAudioClip.length,LoopHighValue*selectedAudioClip.length);
        }
        
        ownerAddPannel.NewPannel(audio);
        Destroy(this.gameObject);

    }

    void Cancel()
    {
        ownerAddPannel.Cancel();
        Destroy(this.gameObject);
    }

    void Browse()
    {
        selectedAudioPath = OpenFileBrowser();

        if(selectedAudioPath != "")
        {
            BrowseField.text = selectedAudioPath;
            
            OpenFileFromPath(selectedAudioPath);

        }
        else
        {
            BrowseField.text = "<color=#FF0000>Not a valid Path!";
        }
        


    }   


    //play the selected audio clip if there is one
    void Play()
    {
        if(selectedAudioClip != null)
        {
            audioSource.clip = selectedAudioClip;
            audioSource.Play();
        }

    }

    void PlayLoop()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Handle File opening
    public string OpenFileBrowser()
    {
        string path = "";

        var bp = new BrowserProperties();
        bp.filter = "Audio File (*.mp3) | *mp3";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, foundPath => {
            path = foundPath;
        });

        return path;
    }

    public void OpenFileFromPath(string path)
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
                selectedAudioClip = null;
                ConfirmButton.interactable = false;
                LoopLowSlider.interactable = false;
                LoopHighSlider.interactable = false;
            }
            else
            {
                selectedAudioClip = DownloadHandlerAudioClip.GetContent(uwr);
                ConfirmButton.interactable = true;
                LoopLowSlider.interactable = true;
                LoopHighSlider.interactable = true;
            }

        }

    }


}
