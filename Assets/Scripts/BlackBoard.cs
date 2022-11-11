using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoard : MonoBehaviour
{
    public bool ShouldBePatrolling;
    public bool ADoorIsOpen;

    public bool eastDoorOpen;
    public GameObject ThiefObject;
    public GameObject ObjectToSteal;
    public GameObject[] ObjectList;
    public Text GuardAlertedText;

    public bool ItemIsStolen;
    public bool ShouldChangeText = true;

    public bool AllGuardsDetected;

    public string RoomWithItem;

    private void Awake()
    {
        ThiefObject = GameObject.FindGameObjectWithTag("Thief");
        int RandomInt = Random.Range(0, 3);
        ObjectList[RandomInt].SetActive(true);
        ObjectToSteal = ObjectList[RandomInt];
        RoomWithItem = ObjectToSteal.GetComponent<Item>().RoomName;

    }

    private void Update()
    {
        if (AllGuardsDetected && ShouldChangeText) 
        {
            GuardAlertedText.text = "Yes";
        }
    }
}
