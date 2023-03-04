using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildUI : MonoBehaviour
{
    [SerializeField] GameObject PannelPrefab;
    
    [SerializeField] List<PanelScript> pannelList;

    [SerializeField] Slider scrollSlider;

    [SerializeField] public AudioScriptableObject audioGroup;


    private int totalLenghtOfCanvas;
    private int maxPannelsPerPage = 12;
    private int maxPannelsPerColumn = 3;

    private int maxPannelsPerRow = 4;


    // Start is called before the first frame update
    void Start()
    {
        pannelList = new List<PanelScript>();
        Vector3 prefPos = new Vector3();

        int totalPannels = audioGroup.audioList.Count;
        totalLenghtOfCanvas = (int)(totalPannels/maxPannelsPerPage) * maxPannelsPerRow * 200;

        for(int i = 0; (int)(totalPannels/maxPannelsPerPage) >= i; i++)
        {
            for(int j = 0; (int)( ( totalPannels - i * maxPannelsPerPage ) / maxPannelsPerColumn) >= j && maxPannelsPerColumn  > j; j++)
            {
                Debug.Log("j:"+j);
                for(int k = 0; (totalPannels - maxPannelsPerPage * i - j * maxPannelsPerRow - k > 0) && maxPannelsPerRow > k; k++)
                {

                   
                    Debug.Log(totalPannels - maxPannelsPerPage * i - j * maxPannelsPerRow - k);
                    
                    
                    var pannel = Instantiate(PannelPrefab);
                    var script = pannel.GetComponent<PanelScript>();

                    pannel.transform.SetParent(transform);
                    pannel.transform.position = new Vector3(i*200*4 + k*200 + 100,800 - j*200 - 100,0);
                    Debug.DrawLine(pannel.transform.position,prefPos,Color.blue,100);

                    prefPos = pannel.transform.position;
                    pannelList.Add(script);
                    script.gridPos = new Vector2(i * 4 + k ,j);
                    script.audioGroup = audioGroup.groupName;
                    script.audioItem = audioGroup.audioList[maxPannelsPerPage * i + j * maxPannelsPerRow + k];
                    //Debug.Log(script.gridPos);
                }
            }

        }


      
              // 
        
        scrollSlider.onValueChanged.AddListener(
            (value)=>
            {
                foreach(PanelScript panelScript in pannelList)
                {
                    float x = panelScript.gridPos.x*200 + 100 - totalLenghtOfCanvas*value;
                    float y =  800 - panelScript.gridPos.y*200 - 100;
                    panelScript.transform.position = new Vector3(x,y,0);

                }

            }
        );

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
