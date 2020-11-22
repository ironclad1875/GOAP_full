using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFSM
{
    //public bool isDoingSomething = false;
    //private string need;


    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    float hunger = gameObject.GetComponent<AiNeeds>().hunger;
    //    float warmth = gameObject.GetComponent<AiNeeds>().warmth;
    //    //float energy = gameObject.GetComponent<AiNeeds>().energy;
    //    if (hunger < warmth)
    //    {
    //        if(gameObject.GetComponent<Backpack>().food > 0)
    //        {
    //            GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("canEat", true);
    //            gameObject.GetComponent<Eat>().eat();
    //            //isDoingSomething = true;
    //        }
    //        else
    //        {
    //            gameObject.GetComponent<Eat>().pickberrys();
    //            //isDoingSomething = true;
    //        }

    //    }
    //    else if (warmth < hunger)
    //    {
    //        GameObject woodpile = GameObject.Find("WoodPile");
    //        GameObject fire = GameObject.Find("CampFire");
    //        if (fire.GetComponent<Campfire>().fireWood <= 0 && gameObject.GetComponent<Backpack>().wood <= 0)
    //        {
    //            gameObject.GetComponent<AIAgent>().targettest = woodpile;
    //        }
    //        else
    //        {
    //            gameObject.GetComponent<WarmUp>().move();
    //        }


    //        //isDoingSomething = true;
    //    }
    //}

    private Stack<WorldState> stateStack = new Stack<WorldState>();

    public delegate void WorldState(MyFSM fsm, GameObject gameObject);


    public void Update(GameObject gameObject)
    {
        if (stateStack.Peek() != null)
            stateStack.Peek().Invoke(this, gameObject);
    }

    public void pushState(WorldState state)
    {
        stateStack.Push(state);
    }

    public void popState()
    {
        stateStack.Pop();
    }

}
