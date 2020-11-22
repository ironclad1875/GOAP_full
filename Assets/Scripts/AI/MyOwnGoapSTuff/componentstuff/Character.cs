using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : AiNeeds
{
    public float hunger = 100;
    public float warmth = 100;

    private void Start()
    {
        //randomize values so each ai has different needs
        hunger = Random.Range(0, 100);
        warmth = Random.Range(0, 100);
    }
    private void Update()
	{
        //having values change so that different goals arise
       
        if (isNextToHeater)
        {
            warmth += Time.deltaTime;
        }
        else
        {
            warmth -= Time.deltaTime;
        }
        if (hunger <= 0)
        {
            hunger = 0;
        }
        
        if (warmth <= 0)
        {
            warmth = 0;
        }
        if (hunger >= 100)
        {
            hunger = 100;
        }
        if (warmth >= 100)
        {
            warmth = 100;
        }
        hunger -= Time.deltaTime ;
    }

	public override HashSet<KeyValuePair<string, object>> createGoalState()
	{
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        
            //iff hungry will collect food and eat
        if(hunger < warmth)
        {
            goal.Add(new KeyValuePair<string, object>("hasEaten", true));
        }
        //if cold will get sticks and start the fire
        else if(warmth < hunger)
        {
            goal.Add(new KeyValuePair<string, object>("warmingUp", true));
        }
        
        
        return goal;
	}
}
