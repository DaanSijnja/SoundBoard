using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildUI : MonoBehaviour
{   

    //Prefabs for easy instanciate
    [SerializeField] GameObject PannelPrefab;
    [SerializeField] GameObject AddPannelPrefab;
    
    //A instance of the AddPannel
    AddPannelScript AddPannel;

    //List of all pannels
    [SerializeField] List<PanelScript> pannelList;

    //The scroll slider
    [SerializeField] Slider scrollSlider;
    
    //The Audio group this tab belongs to   
    [SerializeField] public SoundGroup soundGroup;

    //Some constants for the making of the pannels
    private int totalLenghtOfCanvas;
    private float maxPannelsPerPage = 12f;
    private float maxPannelsPerColumn = 3f;
    private float maxPannelsPerRow = 4f;


    // Start is called before the first frame update
    void Start()
    {
        //Make a new PannelList
        pannelList = new List<PanelScript>();

        //Easy acces for the total pannel count
        int totalPannels = soundGroup.AudioList.Count;

        //Calulate the Canvas length for the scroll function
        totalLenghtOfCanvas = (int)( (int) (totalPannels/maxPannelsPerPage) * maxPannelsPerRow * 200);



        int i = 0;
        int j = 0;
        int k = 0;

        //Check how many Pannel pages there should be and for loop this
        for(i = 0; Mathf.RoundToInt((totalPannels/maxPannelsPerPage) + 0.5f) > i; i++)
        {
            //Check how many colums there are and for loop this, check if there are no more pannels on a column than allowed.
            for(j = 0; Mathf.RoundToInt(( ( totalPannels - i * maxPannelsPerPage ) / maxPannelsPerColumn)+ 0.5f) > j && maxPannelsPerColumn  > j; j++)
            {
                //Check how many pannels there still remaining and if there are not more pannels on a row than allowed
                for(k = 0; (totalPannels - maxPannelsPerPage * i - j * maxPannelsPerRow - k > 0) && maxPannelsPerRow > k; k++)
                {

                    //Instanciate a new Pannel from a prefab and get the PanelScript component
                    var pannel = Instantiate(PannelPrefab);
                    var script = pannel.GetComponent<PanelScript>();

                    //Set the transform of the new pannel
                    pannel.transform.SetParent(transform);
                    pannel.transform.position = new Vector3(i*200*4 + k*200 + 100,800 - j*200 - 100,0);
                    
                    //Set some values to the pannel
                    script.gridPos = new Vector2(i * 4 + k ,j);
                    script.audioGroup = soundGroup.GroupName;
                    script.audioItem = soundGroup.AudioList[(int)(maxPannelsPerPage * i + j * maxPannelsPerRow + k)];


                    //Add the Pannel to the PannelList
                    pannelList.Add(script);
                    
                    
                }
            }

        }

        //Make the Add Button
        var obj = Instantiate(AddPannelPrefab);

        //Set the gridPos of the AddPannel
        AddPannel = obj.GetComponent<AddPannelScript>();
        AddPannel.gridPos = ListPositionToVector2(pannelList.Count);
        AddPannel.UIManager = this;

        //Set the coords and parent to the Add Button 
        obj.transform.SetParent(transform);
        obj.transform.position = new Vector3(AddPannel.gridPos.x*200+100,800-AddPannel.gridPos.y*200-100,0);

      
        //Check the Slider for the Scrollbar
        scrollSlider.onValueChanged.AddListener((_) => { UpdateUI(); } );

    }

    //Add a new pannel to the UI and updates the Add Pannel pannel
    public void AddPannelToUI(Audio newAudio)
    {   
        Vector2 PannelCoords = ListPositionToVector2(pannelList.Count);
        soundGroup.AudioList.Add(newAudio);
        //Instanciate new Pannel
        var pannel = Instantiate(PannelPrefab);
        var script = pannel.GetComponent<PanelScript>();

        //Set the transform of the new pannel
        pannel.transform.SetParent(transform);

        script.gridPos = PannelCoords;
        script.audioGroup = soundGroup.GroupName;
        script.audioItem = newAudio;
        //Add the Pannel to the PannelList
        pannelList.Add(script);

        //Transform the AddPannel
        Vector2 AddPannelCoords = ListPositionToVector2(pannelList.Count);
        AddPannel.gridPos = AddPannelCoords;

        SaveAndLoadManager.Instance.Save(soundGroup);

        UpdateUI();
    }



    public void UpdateUI()
    {
        //Set every UI element to there position.
        foreach(PanelScript panelScript in pannelList)
        {
            float x = panelScript.gridPos.x*200 + 100 - totalLenghtOfCanvas*scrollSlider.value;
            float y =  800 - panelScript.gridPos.y*200 - 100;
            panelScript.transform.position = new Vector3(x,y,0);
        }    

        float _x = AddPannel.gridPos.x*200 + 100 - totalLenghtOfCanvas*scrollSlider.value;
        float _y = 800 - AddPannel.gridPos.y*200 - 100;

        AddPannel.transform.position = new Vector3(_x,_y,0);


    }


    //Converts a grid pos to the List position
    int Vector2ToListPosition(Vector2 gridPos)
    {
        int x = (int)gridPos.x;
        int y = (int)gridPos.y;

        return (int)(Mathf.Floor(x/4)*maxPannelsPerPage + x%maxPannelsPerRow + y*maxPannelsPerRow);
    }

    //Converts a Listposition to a grid pos
    Vector2 ListPositionToVector2(int listPos)
    {
        int x = (int)(Mathf.Floor(listPos/12f)*4 + listPos%4);
        int y = (int)((listPos%12)/4);

        return new Vector2(x,y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
