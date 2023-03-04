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
    [SerializeField] public AudioScriptableObject audioGroup;

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
        int totalPannels = audioGroup.audioList.Count;

        //Calulate the Canvas length for the scroll function
        totalLenghtOfCanvas = (int)( (int) (totalPannels/maxPannelsPerPage) * maxPannelsPerRow * 200);



        int i = 0;
        int j = 0;
        int k = 0;

        //Check how many Pannel pages there should be and for loop this
        for(i = 0; Mathf.RoundToInt((totalPannels/maxPannelsPerPage) + 0.5f) > i; i++)
        {
            //Check how many colums there are and for loop this, check if there are no more pannels on a column than allowed.
            for(j = 0;Mathf.RoundToInt(( ( totalPannels - i * maxPannelsPerPage ) / maxPannelsPerColumn)+ 0.5f) > j && maxPannelsPerColumn  > j; j++)
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
                    script.audioGroup = audioGroup.groupName;
                    script.audioItem = audioGroup.audioList[(int)(maxPannelsPerPage * i + j * maxPannelsPerRow + k)];


                    //Add the Pannel to the PannelList
                    pannelList.Add(script);
                    
                    
                }
            }

        }
        

        //Find the last pannel form the list (this is also the last coords a pannel is placed on)
        var lastPannel = pannelList[pannelList.Count-1];
        float _x = lastPannel.gridPos.x;
        float _y = lastPannel.gridPos.y;

    
        


        //Make the Add Button
        var obj = Instantiate(AddPannelPrefab);

        //Set the gridPos of the AddPannel
        AddPannel = obj.GetComponent<AddPannelScript>();
        AddPannel.gridPos = CalcNextPosInRow();

        //Set the coords and parent to the Add Button 
        obj.transform.SetParent(transform);
        obj.transform.position = new Vector3(AddPannel.gridPos.x*200+100,800-AddPannel.gridPos.y*200-100,0);

      
        //Check the Slider for the Scrollbar
        scrollSlider.onValueChanged.AddListener(
            (value)=>
            {
                //For each panelScript in the List of Pannels
                foreach(PanelScript panelScript in pannelList)
                {
                    //Add the slider value to the X pos and reset the y pos
                    float x = panelScript.gridPos.x*200 + 100 - totalLenghtOfCanvas*value;
                    float y =  800 - panelScript.gridPos.y*200 - 100;
                    panelScript.transform.position = new Vector3(x,y,0);
                }

                float _x = AddPannel.gridPos.x*200 + 100 - totalLenghtOfCanvas*value;
                float _y = 800 - AddPannel.gridPos.y*200 - 100;

                AddPannel.transform.position = new Vector3(_x,_y,0);
                
            }
        );

    }

    //Add a new pannel to the UI and updates the Add Pannel pannel
    public void AddPannelToUI(Audio newAudio)
    {




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

        


    }


    //Calculates which gridpos the next pannel should have
    Vector2 CalcNextPosInRow()
    {
        var lastPannel = pannelList[pannelList.Count-1];
        float _x = lastPannel.gridPos.x;
        float _y = lastPannel.gridPos.y;

        //Fancy stuff for making a new coord for a new pannel
        float x = _x, y = _y;

        if((_x+1) % 4 != 0)
        {
            x = _x + 1;
        }
        else
        {
            x = _x - 3;
            y = _y +1;
        }

        if(_y == 2)
        {
            y = 0;
            x = _x + 1;

        }

        return new Vector2(x,y);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
