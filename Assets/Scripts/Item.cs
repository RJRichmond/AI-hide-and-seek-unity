using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool HasBeenTaken;
    public string RoomName;
    BlackBoard Bboard;
    public Text itemTakenText;

    private void Awake()
    {
        Bboard = GameObject.FindGameObjectWithTag("blackBoard").GetComponent<BlackBoard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Thief" && !HasBeenTaken)
        {
            itemTakenText.text = "Yes";
            HasBeenTaken = true;
            other.gameObject.GetComponent<ThiefAi>().IsAtItem = true;
            other.gameObject.GetComponent<ThiefAi>().ShouldChangeState = true;
        }
        else if (other.gameObject.tag == "Guard")
        {
            other.gameObject.GetComponent<GuardAI>().ShouldBePatrolling = true;
            other.gameObject.GetComponent<GuardAI>().HasInvestigatedItem = true;
            if (HasBeenTaken) 
            {
                Bboard.ItemIsStolen = true;
                //Debug.Log("Item stolen and been checked.");
            }
        }
    }
}
