using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiNeeds : MonoBehaviour, MyGOAP
{

    //public float hunger = 100;
    //public float energy = 100;
    //public float warmth = 100;
    public bool isResting = false;
    public bool isNextToHeater = false;
    public bool hasFood = false;

    public Backpack backpack;
    public GameObject character;
    public float moveSpeed =1;

    // Start is called before the first frame update
    void Start()
    {
        if (backpack == null)
            backpack = gameObject.AddComponent<Backpack>() as Backpack;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        

    }
    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        Character charactercomp = character.GetComponent<Character>(); 
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasSticks", (backpack.wood > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasFood", (backpack.food > 0)));
        worldData.Add(new KeyValuePair<string, object>("isCold", (charactercomp.warmth < charactercomp.hunger)));
        worldData.Add(new KeyValuePair<string, object>("isHungry", (charactercomp.hunger < charactercomp.warmth)));

        

        return worldData;
    }
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();

    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.
    }
    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<Action> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + AIAgent.prettyPrint(actions));
    }
    public void actionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }
    public void planAborted(Action aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + AIAgent.prettyPrint(aborter));
    }

    public bool moveAgent(Action nextAction)
    {
        // move towards the NextAction's target
        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

        //if (gameObject.transform.position.Equals(nextAction.target.transform.position))
        //{
        if(Vector3.Distance(gameObject.transform.position, nextAction.target.transform.position) <= 1)
        {
            // we are at the target location, we are done
            nextAction.setInRange(true);
            return true;
        }
            
        //}
        else
            return false;
    }
    
    
        
    
}
