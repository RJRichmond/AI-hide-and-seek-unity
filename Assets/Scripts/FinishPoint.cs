using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Thief")
        {
            Debug.Log("Simulation complete");
            SceneManager.LoadScene(0);
        }
    }
}
