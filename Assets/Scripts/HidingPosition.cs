using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPosition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Thief") 
        {
            other.GetComponent<ThiefAi>().IsDetected = false;
            other.GetComponent<ThiefAi>().ShouldFindItem = true;
            other.GetComponent<ThiefAi>().ShouldChangeState = true;
        }
    }
}
