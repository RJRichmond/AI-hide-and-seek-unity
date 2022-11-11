using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardVision : MonoBehaviour
{

    public bool CanSeeThief;
    public BlackBoard Bboard;
    private void Awake()
    {
        Bboard = GameObject.FindGameObjectWithTag("blackBoard").GetComponent<BlackBoard>();
    }

    private void Update()
    {
       // Debug.Log("this happens right");
        RaycastHit Ray;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Ray, 100f)) 
        {
            if (Ray.collider.gameObject.tag == "Thief")
            {
               // Debug.Log("Thief was hit");
                this.gameObject.transform.parent.parent.GetComponent<GuardAI>().ThiefDetected = true;
                if (Bboard.ItemIsStolen) 
                {
                    Bboard.AllGuardsDetected = true;
                }
                Ray.collider.gameObject.GetComponent<ThiefAi>().IsDetected = true;
                Ray.collider.gameObject.GetComponent<ThiefAi>().ShouldChangeState = true;
                CanSeeThief = true;
            }
            else 
            {
                CanSeeThief = false;
            }
        
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Ray.distance, Color.yellow);

        if (this.gameObject.transform.parent.parent.GetComponent<GuardAI>().ThiefDetected && !CanSeeThief) 
        {
            StartCoroutine(LostSpyTimer());
        }

    }

    IEnumerator LostSpyTimer()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.transform.parent.parent.GetComponent<GuardAI>().ThiefDetected = false;
    }
}
