using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;


public class SaveAndLoadManager : MonoBehaviour
{
    public string fileName;
    public SaveData saveData;
    [SerializeField] GameObject TabPrefab;
    [SerializeField] GameObject TabButtonPrefab;
    [SerializeField] Transform ButtonMask;
    [SerializeField] List<BuildUI> tabList;
    [SerializeField] List<GameObject> tabButtonList;

    private GameObject AddTabButton;

    //Editing the groupname
    [SerializeField] TMP_Text groupNameText;
    [SerializeField] GameObject groupnameEditHolder;
    [SerializeField] Button editGroupNameButton;
    [SerializeField] TMP_InputField groupNameInputfield;
    [SerializeField] Button groupNameInputDone;

    //TabSlider and Active Tab
    [SerializeField] Slider TabSlider;

    [SerializeField] BuildUI activeTab;

    string filepath;

    public static SaveAndLoadManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.persistentDataPath + "/" + saveData +".txt";
        tabList = new List<BuildUI>();

        if(Instance == null)
            Instance = this;

        TabSlider.onValueChanged.AddListener((_) => UpdateTabButtons());
        groupnameEditHolder.SetActive(false);

        editGroupNameButton.onClick.AddListener(() => groupnameEditHolder.SetActive(true));
        groupNameInputDone.onClick.AddListener(() => SaveTabNameChange(groupNameInputfield.text));


        Load();
    }   


    public void Load()
    {
        if(File.Exists(filepath))
        {   
            string loadedString = File.ReadAllText(filepath);
            saveData = JsonUtility.FromJson<SaveData>(loadedString);



        }
        else
        {
            saveData = new SaveData();
        }
        int i = 0;
        //Make for every soundgroup a tab and a tab button
        foreach(SoundGroup soundGroup in saveData.SoundGroups)
        {
            AddSoundGroup(soundGroup,i,false);
            i++;
        }

        AddTabButton = Instantiate(TabButtonPrefab);
        AddTabButton.transform.SetParent(ButtonMask);
        AddTabButton.transform.localPosition = new Vector3(14,68-i*34,0);
        AddTabButton.GetComponentInChildren<TMP_Text>().text = "Add Group";
            
        AddTabButton.GetComponent<Button>().onClick.AddListener(() => {AddSoundGroup(new SoundGroup("New Tab"),(tabList.Count),true); UpdateTabButtons();});

        
    }

    void SetActiveTab(BuildUI tabScript)
    {
        if(activeTab != null)
            activeTab.gameObject.SetActive(false);

        groupNameText.text = tabScript.soundGroup.GroupName;
        activeTab = tabScript;
        tabScript.gameObject.SetActive(true);

    }

    void AddSoundGroup(SoundGroup soundGroup, int location,bool addToList)
    {   

        if(addToList)
        {
            saveData.SoundGroups.Add(soundGroup);
        }

        var Tab = Instantiate(TabPrefab);
        Tab.transform.SetParent(this.transform);
        Tab.transform.localPosition = new Vector3(0,0,0);

        BuildUI TabScript = Tab.GetComponent<BuildUI>();
        tabList.Add(TabScript);
        TabScript.soundGroup = soundGroup;

        Tab.SetActive(false);

        var button = Instantiate(TabButtonPrefab);
        button.transform.SetParent(ButtonMask);
        button.transform.localPosition = new Vector3(14,68-location*34,0);
        button.GetComponentInChildren<TMP_Text>().text = soundGroup.GroupName;
        
        tabButtonList.Add(button);

        button.GetComponent<Button>().onClick.AddListener(() => SetActiveTab(TabScript));

        SetActiveTab(TabScript);
    }

    void UpdateTabButtons()
    {
        int i = 0;
        float listLenght = (tabButtonList.Count)*30;

        foreach(GameObject button in tabButtonList)
        {
            button.transform.localPosition = new Vector3(14,68-i*34 + listLenght*TabSlider.value,0);
            i++;
        }

        AddTabButton.transform.localPosition = new Vector3(14,68-i*34 + listLenght*TabSlider.value,0);

    }


    public void Save(SoundGroup soundGroup)
    {

        for(int i = 0; i < saveData.SoundGroups.Count; i++)
        {
            if(saveData.SoundGroups[i].customID == soundGroup.customID)
            {
                saveData.SoundGroups[i] = soundGroup;

            }

        }

        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(filepath, json);

    }

    void SaveTabNameChange(string _name)
    {
        activeTab.soundGroup.GroupName = _name;
        var soundGroup = activeTab.soundGroup;

        for(int i = 0; i < saveData.SoundGroups.Count; i++)
        {
            if(saveData.SoundGroups[i].customID == soundGroup.customID)
            {
                tabButtonList[i].GetComponentInChildren<TMP_Text>().text = _name;

            }

        }

        groupnameEditHolder.SetActive(false);

        Save(soundGroup);

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    



}

[System.Serializable]
public class SaveData
{
    public List<SoundGroup> SoundGroups;

    public SaveData()
    {
        SoundGroups = new List<SoundGroup>();
    }
}


[System.Serializable]
public class SoundGroup
{
    public int _customID;
    public int customID {get => _customID; }
    public string GroupName;
    public List<Audio> AudioList;

    public SoundGroup(string _name)
    {
        GroupName = _name;
        _customID = this.GetHashCode();
        AudioList = new List<Audio>();
    }
}
