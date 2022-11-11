using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThiefAi : MonoBehaviour
{
    StateMachine ThiefStateMachine;
    GameObject Bboard;
    public GameObject EndPosition;
    public GameObject[] HidingPositions;
    public GameObject CurrentHidingPosition;

    public bool IsDetected;
    public bool IsAtItem;
    public bool HasItem;
    public bool ShouldFindItem;
    public bool ShouldChangeState;

    public int ItemTakePercentage;
    public Text GuardText;


    private void Awake()
    {
        ThiefStateMachine = new StateMachine();
        Bboard = GameObject.FindGameObjectWithTag("blackBoard");

        ThiefStateMachine.ChangeCurrentState(new FindItemState(),this.gameObject);

    }

    private void Update()
    {
        if (ShouldChangeState) 
        {
            ThiefStateMachine.CheckWhichState(this.gameObject, ThiefStateMachine);
        }
        ThiefStateMachine.Update(this.gameObject, Bboard);


    }
}

public class State
{
    public virtual void EnterState(GameObject thief)
    {

    }
    public virtual void PerformState(GameObject thief, GameObject blackBoard) 
    {
    
    }
    public virtual void ExitState() 
    {
        
    }
}

public class StateMachine 
{
    State CurrentState;

    public void ChangeCurrentState(State newState, GameObject thief) 
    {
        if (CurrentState != null) 
        {
            CurrentState.ExitState();
        }
        CurrentState = newState;
        CurrentState.EnterState(thief);
    }

    public void CheckWhichState(GameObject Thief, StateMachine thiefStateMachine) 
    {
        if (Thief.gameObject.GetComponent<ThiefAi>().IsDetected)
            thiefStateMachine.ChangeCurrentState(new Flee(),Thief);
        else if (Thief.gameObject.GetComponent<ThiefAi>().IsAtItem)
            thiefStateMachine.ChangeCurrentState(new InteractItemState(), Thief);
        else if (Thief.gameObject.GetComponent<ThiefAi>().HasItem)
            thiefStateMachine.ChangeCurrentState(new Escape(), Thief);
        else if (Thief.gameObject.GetComponent<ThiefAi>().ShouldFindItem)
            thiefStateMachine.ChangeCurrentState(new FindItemState(), Thief);
    }

    public void Update(GameObject thief, GameObject blackBoard) 
    {
        if (CurrentState != null) 
        {
            CurrentState.PerformState(thief, blackBoard);
        }
    }
}

//make stats here

public class FindItemState : State 
{
    public override void EnterState(GameObject thief) 
    {
       // Debug.Log("Entered finding item state");
    }
    public override void PerformState(GameObject thief, GameObject blackBoard)
    {
        //  Debug.Log("Going to find the item by pathing to it");
        thief.GetComponent<ThiefAi>().GuardText.text = "Finding item";
        thief.GetComponent<NavMeshAgent>().speed = 6;
        thief.GetComponent<NavMeshAgent>().destination = blackBoard.GetComponent<BlackBoard>().ObjectToSteal.transform.position;


    }
    public override void ExitState()
    {
       // Debug.Log("Exiting the find item state");
    }
}

public class InteractItemState : State 
{
    public override void EnterState(GameObject thief)
    {
       // Debug.Log("Entered stealing item state");
    }
    public override void PerformState(GameObject thief, GameObject blackBoard)
    {
        // Debug.Log("going to interact with the item");
        thief.GetComponent<ThiefAi>().GuardText.text = "Stealing item";
        thief.GetComponent<ThiefAi>().ItemTakePercentage += 1;
        if (thief.GetComponent<ThiefAi>().ItemTakePercentage >= 1000) 
        {
            thief.GetComponent<ThiefAi>().HasItem = true;
            thief.GetComponent<ThiefAi>().IsAtItem = false;
            thief.GetComponent<ThiefAi>().ShouldChangeState = true;
        }
    }
    public override void ExitState()
    {
      //  Debug.Log("Exiting interaction state");
    }
}

public class Flee : State 
{
    public override void EnterState(GameObject thief)
    {
       // Debug.Log("Entered finding flee state");
        float lowestDistance = Mathf.Infinity;

        for (int i = 0; i < thief.GetComponent<ThiefAi>().HidingPositions.Length; i++)
        {
            float distance = Vector3.Distance(thief.GetComponent<ThiefAi>().HidingPositions[i].transform.position, thief.transform.position);
            if (distance < lowestDistance)
            {
                lowestDistance = distance;
                thief.GetComponent<ThiefAi>().CurrentHidingPosition = thief.GetComponent<ThiefAi>().HidingPositions[i];
            }
        }
    }
    public override void PerformState(GameObject thief, GameObject blackBoard)
    {
        // Debug.Log("Going to escape to a hiding spot");
        thief.GetComponent<ThiefAi>().GuardText.text = "Fleeing from guards";
        thief.GetComponent<NavMeshAgent>().speed = 8;
        thief.GetComponent<NavMeshAgent>().destination = thief.GetComponent<ThiefAi>().CurrentHidingPosition.transform.position;
    }
    public override void ExitState()
    {
       // Debug.Log("Exiting the fleeing state");
    }
}

public class Escape : State
{
    public override void EnterState(GameObject thief)
    {
      //  Debug.Log("Entered escape state, item has been aqquired");
    }
    public override void PerformState(GameObject thief, GameObject blackBoard)
    {
        //  Debug.Log("Going to escape the building objective complete");
        thief.GetComponent<ThiefAi>().GuardText.text = "Escaping with the item";
        thief.GetComponent<NavMeshAgent>().destination = thief.GetComponent<ThiefAi>().EndPosition.transform.position;
    }
    public override void ExitState()
    {
      //  Debug.Log("Exiting the escape state");
    }
}

