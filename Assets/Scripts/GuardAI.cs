using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GuardAI : MonoBehaviour
{
    //Variables
    NavMeshAgent GuardAgent;

    public Vector3[] PatrolPathPoints;

    public bool Patrol;
    public bool DoorNearOpen;
    public bool ThiefDetected;
    public bool HasInvestigatedItem;
    public bool ShouldBePatrolling = true;

    public int CurrentPoint;
    public int PatrolCount;
    public int PatrolItemCount;

    private BlackBoard blackBoard;
    public GameObject PathObjects;
    public GameObject DoorInvestigating;
    

    public string CurrentRoom;


    Selector RootNode;
    Sequence PatrolSequence; 
    NormalPatrolRoute PatrolTask;
    CheckDoor DoorCheckTask;
    Aggressive AggressiveTask;
    InspectObject CheckItemTask;
    SucceederNode SucceederForDoorCheck;

    private void Start()
    {
        //Setting path for the patrol
        for (int i = 0; i < 4; i++) 
        {
            PatrolPathPoints[i] = PathObjects.transform.GetChild(i).gameObject.transform.position;
        }

        //Assembling bevehaviour tree starting with root node.
        blackBoard = GameObject.FindGameObjectWithTag("blackBoard").GetComponent<BlackBoard>();

        RootNode = new Selector();

        PatrolSequence = new Sequence();

        

        //Making the patrolling task and then setting up the task by feeding the variables needed.
        PatrolTask = new NormalPatrolRoute();
        PatrolTask.CheckIfPatrolling(blackBoard, this.gameObject);

        SucceederForDoorCheck = new SucceederNode();

        DoorCheckTask = new CheckDoor();
        DoorCheckTask.GetVariables(blackBoard, this.gameObject);

        AggressiveTask = new Aggressive();
        AggressiveTask.GetVariables(blackBoard, this.gameObject);

        CheckItemTask = new InspectObject();
        CheckItemTask.GetVariables(blackBoard, this.gameObject);

        RootNode.AddChildToList(AggressiveTask);
        RootNode.AddChildToList(CheckItemTask);
        RootNode.AddChildToList(PatrolSequence);

        SucceederForDoorCheck.SetChildNode(DoorCheckTask);

        PatrolSequence.AddChildToList(SucceederForDoorCheck);
        PatrolSequence.AddChildToList(PatrolTask);

    }

    private void Update()
    {
        while (!RootNode.Tick()) 
        {
            //Debug.Log("Bevehaviour Tree still working");
        }
        
    }

   

  

}


