using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioScriptableObject", order = 1)]
public class AudioScriptableObject : ScriptableObject
{
    public string groupName;
    public List<Audio> audioList;

}
