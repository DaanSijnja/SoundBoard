using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using TMPro;

public class NewPannelInput : MonoBehaviour, ILoopInput
{

    [SerializeField] TMP_InputField audioName;
    //Browse Inputs
    [SerializeField] TMP_InputField BrowseField;
    [SerializeField] Button BrowseButton;

  

    //Buttons
    [SerializeField] Button PlayButton;
    [SerializeField] Button ConfirmButton;
    [SerializeField] Button CancelButton;
    [SerializeField] Button AddLoopButton;

    [SerializeField] AudioSource audioSource;

    public AudioClip selectedAudioClip;
    private string selectedAudioPath;

    private List<Loop> loops;
    private List<GameObject> loopDisplays;
    
    public GameObject AddLoopPrefab;
    public GameObject LoopDisplayPrefab;
    public INewPannelInput newPannelInputOwner;

    // Start is called before the first frame update
    void Start()
    {
        loops = new List<Loop>();
        loopDisplays = new List<GameObject>();
        
        //Onclicks for the buttons
        ConfirmButton.onClick.AddListener(() => Confirm());
        BrowseButton.onClick.AddListener(() =>  Browse());
        CancelButton.onClick.AddListener(() => Cancel());
        PlayButton.onClick.AddListener(() => Play());
        AddLoopButton.onClick.AddListener(() => OpenAddLoop());


        

    }

    public void LoopInputConfirm(Loop loop)
    {   
        if(loop.loopName != "" && loops.Count < 3)
        {
            //Loop Dislay Stuff
            var obj = Instantiate(LoopDisplayPrefab);
            obj.transform.SetParent(this.transform);
            obj.transform.localPosition = new Vector3(0,0-loops.Count*40-25,0);
            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = loop.loopName;

            //Remove Button
            obj.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
                {
                    loops.Remove(loops[loopDisplays.IndexOf(obj)]);
                    loopDisplays.Remove(obj);
                    int i = 0;
                    foreach(GameObject gameObj in loopDisplays)
                    {
                        gameObj.transform.localPosition = new Vector3(0,0-i*40-25,0); 
                        i++;
                    }

                    Destroy(obj.gameObject);
                }
            );

            //Edit button
            obj.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    var input = Instantiate(AddLoopPrefab);
                    input.transform.SetParent(this.transform);
                    input.transform.localPosition = Vector3.zero;
                    var script = input.GetComponent<AddLoopScript>();
                    script.selectedAudioClip = selectedAudioClip;
                    script.loopInput = GetComponent<ILoopInput>();
                    script.EditLoop(loops[loopDisplays.IndexOf(obj)],loopDisplays.IndexOf(obj));                  

                }
            );

            loopDisplays.Add(obj);
            loops.Add(loop);
        }
            
    }

    public void LoopInputEdit(int loopIndex, Loop editedLoop)
    {
        if(editedLoop.loopName != "")
        {
            loops[loopIndex] = editedLoop;
            loopDisplays[loopIndex].transform.GetChild(0).GetComponent<TMP_Text>().text = editedLoop.loopName;
        }
    }


    //Gets all the values and makes a new Audio Class object
    void Confirm()
    {
        var audio = new Audio();
        audio.audioName = audioName.text;
        audio.SetAudioClip(selectedAudioClip);

        audio.audioPath = selectedAudioPath;
        audio.Loops = loops;
        
        
        newPannelInputOwner.EditPannel(audio);
        Destroy(this.gameObject);

    }

    void Cancel()
    {
        newPannelInputOwner.CancelPannel();
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

    void OpenAddLoop()
    {
        var obj = Instantiate(AddLoopPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = Vector3.zero;
        var script = obj.GetComponent<AddLoopScript>();
        script.selectedAudioClip = selectedAudioClip;
        script.loopInput = GetComponent<ILoopInput>();
        
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
                PlayButton.interactable = false;
                AddLoopButton.interactable = false;
                
            }
            else
            {
                selectedAudioClip = DownloadHandlerAudioClip.GetContent(uwr);
                ConfirmButton.interactable = true;
                PlayButton.interactable = true;
                AddLoopButton.interactable = true;
            }

        }

    }

}

public interface INewPannelInput
{
    public void EditPannel(Audio audio);
    public void CancelPannel();
}
