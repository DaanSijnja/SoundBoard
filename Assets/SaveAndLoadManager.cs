using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveAndLoadManager : MonoBehaviour
{
    public string fileName;
    public SoundGroup saveData;
    public GameObject TabPrefab;

    string filepath;

    public static SaveAndLoadManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.dataPath + "/" + saveData +".txt";

        if(Instance == null)
            Instance = this;

        Load();
    }   


    public void Load()
    {
        if(File.Exists(filepath))
        {   
            string loadedString = File.ReadAllText(filepath);
            saveData = JsonUtility.FromJson<SoundGroup>(loadedString);



        }
        else
        {
            saveData = new SoundGroup("Music");
        }

        var Tab = Instantiate(TabPrefab);
        Tab.transform.SetParent(this.transform);

        BuildUI TabScript = Tab.GetComponent<BuildUI>();
        TabScript.soundGroup = saveData;
    }

    public void Save(SoundGroup soundGroup)
    {
        string json = JsonUtility.ToJson(soundGroup);
        File.WriteAllText(filepath, json);

    }



    // Update is called once per frame
    void Update()
    {
        
    }


    



}

[System.Serializable]
public class SoundGroup
{
    public string GroupName;
    public List<Audio> AudioList;

    public SoundGroup(string _name)
    {
        GroupName = _name;
        AudioList = new List<Audio>();
    }
}
