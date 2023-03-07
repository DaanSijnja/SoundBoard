using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPannelScript : MonoBehaviour, INewPannelInput
{      
    //Gridpos and UI Manager
    [SerializeField] public Vector2 gridPos;
    [SerializeField] public BuildUI UIManager;

    //New Pannel Input UI element
    [SerializeField] GameObject NewPanneInputPrefab;
    [SerializeField] NewPannelInput CurrentInput;
    //Add Button
    [SerializeField] Button AddButton;



    // Start is called before the first frame update
    void Start()
    {
        AddButton.onClick.AddListener(() => OpenNewPannelWindow());



    }

    //Opens the New Pannel Input Menu
    void OpenNewPannelWindow()
    {
        if(CurrentInput == null)
        {
            var obj = Instantiate(NewPanneInputPrefab);
            
            CurrentInput = obj.GetComponent<NewPannelInput>();
            CurrentInput.newPannelInputOwner = GetComponent<INewPannelInput>();
            CurrentInput.transform.SetParent(transform.parent);
            obj.transform.localPosition = Vector3.zero;
        }
    }
    
    public void EditPannel(Audio audio)
    {

        Debug.Log(audio);
        UIManager.AddPannelToUI(audio);
    }

    public void CancelPannel()
    {
        CurrentInput = null;
    }



}
