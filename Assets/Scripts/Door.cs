using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    private bool RunCoroutineOnce;
    public GameObject PathPoints;
    public string RoomName;
    public Material[] MaterialList;

    private void Awake()
    {
        int RandomInt = Random.Range(0, 2);
        if (RandomInt == 1)
        {
            IsOpen = true;
        }
        else 
        {
            IsOpen = false;
        }
    }
    private void Update()
    {
        if (IsOpen)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<Renderer>().material = MaterialList[1];
        }
        else if(!IsOpen) 
        {
            this.GetComponent<Renderer>().material = MaterialList[0];
            StartCoroutine(BoxColliderForDoor());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Thief") 
        {
            IsOpen = true;
        }
    }

    IEnumerator BoxColliderForDoor()
    {
        if (!RunCoroutineOnce) 
        {
            RunCoroutineOnce = true;
            Debug.Log("This happens");
            yield return new WaitForSeconds(10f);
            this.GetComponent<BoxCollider>().enabled = true;
            RunCoroutineOnce = false;
        }
    }
}
