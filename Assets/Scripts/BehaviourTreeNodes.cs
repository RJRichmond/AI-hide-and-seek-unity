using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Node
{
    //This is the base node for the Behaviour tree
    public virtual bool Tick()
    {
        return false;
    }

}

public class CompositeNode :  Node
{
    private List<Node> Children = new List<Node>();

    public List<Node> ReturnChildList() 
    {
        return Children;
    }

    public void AddChildToList(Node Child) 
    {
        Children.Add(Child);
    }
}

public class Selector : CompositeNode
{
    public override bool Tick()
    {
        
        foreach (Node Child in ReturnChildList()) 
        {
            if (Child.Tick()) 
            {
                return true;
            }
        }
        return false;
    }
}

public class Sequence : CompositeNode 
{
    public override bool Tick()
    {
        
        foreach (Node Child in ReturnChildList()) 
        {
            if (!Child.Tick()) 
            {
                return false;
            }
        }
        return true;

    }
}

public class Decorator : Node 
{
    protected Node Child;
    protected Node GetChild() 
    {
        return Child;
    }
    public void SetChildNode(Node NewChild) 
    {
        Child = NewChild;
    }   
}

public class InverterNode : Decorator
{
    public override bool Tick()
    {
        return !GetChild().Tick();
    }
}

public class SucceederNode : Decorator 
{
    public override bool Tick()
    {
        GetChild().Tick();
        return true;
    }
}

public class NormalPatrolRoute : Node
{
    private BlackBoard blackBoard;
    private GameObject GuardObject;
    private NavMeshAgent GuardAgent;
    private GuardAI GuardScript;
    int failsafe = new int();

    public void CheckIfPatrolling(BlackBoard BBoard,GameObject Guard) 
    {
        blackBoard = BBoard;
        GuardObject = Guard;
        GuardAgent = GuardObject.GetComponent<NavMeshAgent>();
        GuardScript = GuardObject.GetComponent<GuardAI>();
        
    }

    public override bool Tick()
    {
        if (blackBoard.ItemIsStolen) 
        {
            GuardAgent.speed = 8;
        }
        if (GuardScript.ShouldBePatrolling)
        {
            
            //Checking to see if the guard is at the current path point and then to change it if it is.
            if (GuardAgent.transform.position.x <= GuardObject.GetComponent<GuardAI>().PatrolPathPoints[GuardScript.CurrentPoint].x +1 && GuardAgent.transform.position.x >= GuardObject.GetComponent<GuardAI>().PatrolPathPoints[GuardScript.CurrentPoint].x - 1) 
            {
                if (GuardAgent.transform.position.z <= GuardObject.GetComponent<GuardAI>().PatrolPathPoints[GuardScript.CurrentPoint].z + 1 && GuardAgent.transform.position.z >= GuardObject.GetComponent<GuardAI>().PatrolPathPoints[GuardScript.CurrentPoint].z - 1) 
                {
                    GuardObject.GetComponent<GuardAI>().CurrentPoint += 1;
                    if (GuardObject.GetComponent<GuardAI>().CurrentPoint == 4)
                    {
                        GuardObject.GetComponent<GuardAI>().PatrolItemCount += 1;
                        GuardObject.GetComponent<GuardAI>().CurrentPoint -= 4;
                    }
                }
            }
            Debug.Log("The guard should be patrolling as normal");
            //In here we are going to making it so the navmesh agent follows a set path of points by default.
            GuardAgent.destination = GuardObject.GetComponent<GuardAI>().PatrolPathPoints[GuardScript.CurrentPoint];
           
            if (GuardScript.PatrolItemCount >= 2) 
            {
                GuardScript.HasInvestigatedItem = false;
            }

            return true;
        }
        else 
        {
           // Debug.Log("This means something else has happened and the guard will not be partolling");
            return false;
        }
    }
}

public class CheckDoor : Node 
{
    private BlackBoard blackBoard;
    private GameObject GuardObject;
    private NavMeshAgent GuardAgent;
    private GuardAI GuardScript;

    public void GetVariables(BlackBoard BBoard, GameObject Guard) 
    {
        blackBoard = BBoard;
        GuardObject = Guard;
        GuardAgent = GuardObject.GetComponent<NavMeshAgent>();
        GuardScript = GuardObject.GetComponent<GuardAI>();
    }


     public override bool Tick()
    {
        if (GuardScript.DoorNearOpen)
        {
            Debug.Log("A door is nearby");
            GuardScript.PathObjects = GuardScript.DoorInvestigating.GetComponent<Door>().PathPoints;
            for (int i = 0; i < 4; i++)
            {
                GuardScript.PatrolPathPoints[i] = GuardScript.PathObjects.transform.GetChild(i).gameObject.transform.position;
            }
            GuardScript.DoorInvestigating.GetComponent<Door>().IsOpen = false;
            GuardScript.DoorInvestigating.GetComponent<NavMeshObstacle>().enabled = false;
            GuardScript.DoorInvestigating.GetComponent<BoxCollider>().enabled = false;
            GuardScript.DoorNearOpen = false;
            GuardScript.CurrentRoom = GuardScript.DoorInvestigating.GetComponent<Door>().RoomName;
            GuardScript.CurrentPoint = 0;
            GuardScript.HasInvestigatedItem = false;
            return true;
        }
        else 
        {
            return false;
        }
    }

}

public class Aggressive : Node 
{
    private BlackBoard blackBoard;
    private GameObject GuardObject;
    private NavMeshAgent GuardAgent;
    private GuardAI GuardScript;
    private ThiefAi ThiefScript;

    public void GetVariables(BlackBoard BBoard, GameObject Guard) 
    {
        blackBoard = BBoard;
        GuardObject = Guard;
        GuardAgent = GuardObject.GetComponent<NavMeshAgent>();
        GuardScript = GuardObject.GetComponent<GuardAI>();
        ThiefScript = BBoard.ThiefObject.GetComponent<ThiefAi>();
    }
    public override bool Tick()
    {
        if (GuardScript.ThiefDetected || blackBoard.AllGuardsDetected)
        {
          //  Debug.Log("Thief has been detected");
            GuardAgent.speed = 10;
            GuardAgent.destination = blackBoard.ThiefObject.transform.position;
            return true;
        }
        else 
        {
            return false;
        }
    }

}

public class InspectObject : Node 
{
    private BlackBoard blackBoard;
    private GameObject GuardObject;
    private NavMeshAgent GuardAgent;
    private GuardAI GuardScript;

    public void GetVariables(BlackBoard BBoard, GameObject Guard)
    {
        blackBoard = BBoard;
        GuardObject = Guard;
        GuardAgent = GuardObject.GetComponent<NavMeshAgent>();
        GuardScript = GuardObject.GetComponent<GuardAI>();
    }

    public override bool Tick()
    {
        if (GuardScript.CurrentRoom == blackBoard.RoomWithItem && !GuardScript.HasInvestigatedItem) 
        {
          //  Debug.Log("The guard is in the room with the item and is going to check it.");
            GuardAgent.destination = blackBoard.ObjectToSteal.transform.position;
            GuardScript.ShouldBePatrolling = false;
            return true;
        }
        else 
        {
            return false;    
        }
    }
}

