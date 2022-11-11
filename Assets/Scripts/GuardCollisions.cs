using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardCollisions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            if (other.gameObject.GetComponent<Door>().IsOpen)
            {
                this.gameObject.transform.parent.GetComponent<GuardAI>().DoorNearOpen = true;
                this.gameObject.transform.parent.GetComponent<GuardAI>().DoorInvestigating = other.gameObject;
            }
        }
        if (other.gameObject.tag == "Thief")
            SceneManager.LoadScene(0);
    }
}
