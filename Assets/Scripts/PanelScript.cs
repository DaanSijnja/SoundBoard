using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class PanelScript : MonoBehaviour, INewPannelInput
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
    //Edit button
    [SerializeField] Button EditButton;
    //Play Button
    [SerializeField] Button PlayButton;
    //Button prefab for the loops
    [SerializeField] GameObject LoopButtonPrefab;
    [SerializeField] GameObject PannelEditorPrefab;

    private List<GameObject> loopButtons;

    [SerializeField] public BuildUI uiowner;


    //The Audio source
    [SerializeField] GameObject AudioSourceObjectPrefab;
    [SerializeField] AudioSourceScript CurrentASScript;

    // Start is called before the first frame update
    void Start()
    {   
        loopButtons = new List<GameObject>();
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
                obj.transform.localPosition = new Vector3(0,-17.9f + i*30,0);

                //Add an addlistener for a button press
                obj.GetComponent<Button>().onClick.AddListener( () => PlayLoop(loop) );


                loopButtons.Add(obj);

                
                i++;
            }

        }
       

        //Add Listeners to the Play and End Button
        PlayButton.onClick.AddListener( () => PlayFull() );
        EndButton.onClick.AddListener( () => Stop() );
        EditButton.onClick.AddListener( () => OpenPannelEdit() );


    }

    public void EditPannel(Audio audio)
    {
        uiowner.soundGroup.AudioList[uiowner.Vector2ToListPosition(gridPos)] = audio;
        SaveAndLoadManager.Instance.Save(uiowner.soundGroup);
        nameAudio.text = audio.audioName;
        audioItem = audio;

        foreach(GameObject gameobj in loopButtons)
        {
            
            Destroy(gameobj);

        }

        loopButtons.Clear();

        OpenAudioFromPath(audioItem.audioPath);
            //Loop for each Loop in the audio item
        int i = 0;
        foreach(Loop loop in audio.Loops)
        {   
            //Max 3 loops on display
            if(i >= 3)
                break;
            //Instanciate the new Loop button and set some values
            var obj = Instantiate(LoopButtonPrefab);
            obj.GetComponentInChildren<TMP_Text>().text = loop.loopName;
            obj.transform.SetParent(transform);
            obj.transform.localPosition = new Vector3(0,-17.9f + i*30,0);
            //Add an addlistener for a button press
            obj.GetComponent<Button>().onClick.AddListener( () => PlayLoop(loop) );
            loopButtons.Add(obj);
            
            i++;
        }

    }   

    public void CancelPannel()
    {

    }  

    public void RemovePannel()
    {
        uiowner.RemovePannel(this);

    }


    public void OpenPannelEdit()
    {
        var obj = Instantiate(PannelEditorPrefab);
        obj.transform.SetParent(transform.parent);
        obj.transform.localPosition = Vector3.zero;

        var script = obj.GetComponent<NewPannelInput>();
        script.newPannelInputOwner = GetComponent<INewPannelInput>();
        script.EditAudio(audioItem);

    }


    //Play the audio normal and adds sets the Loop
    public void PlayLoop(Loop loop)
    {
        if(CurrentASScript == null)
        {
            var obj = Instantiate(AudioSourceObjectPrefab);
            CurrentASScript = obj.GetComponent<AudioSourceScript>();
            CurrentASScript.audioClip = audioItem.GetAudioClip();
            CurrentASScript.owner = this;
            CurrentASScript.playLoopOnStart = true;
        }

        CurrentASScript.PlayLoop(loop);
    }

    //Play the full audio
    public void PlayFull()
    {
        if(CurrentASScript == null)
        {
            var obj = Instantiate(AudioSourceObjectPrefab);
            CurrentASScript = obj.GetComponent<AudioSourceScript>();
            CurrentASScript.audioClip = audioItem.GetAudioClip();
            CurrentASScript.owner = this;
            CurrentASScript.playFullOnStart = true;
        }

        CurrentASScript.PlayFull();
    }

    //Stops the audio   
    public void Stop()
    {  
        if(CurrentASScript != null)
        {
            CurrentASScript.Stop();
        }
    }   

    public void StoppedPlaying()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
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
                //error
                
            }
            else
            {
                audioItem.SetAudioClip(DownloadHandlerAudioClip.GetContent(uwr));
    

            }

        }
    }

}
