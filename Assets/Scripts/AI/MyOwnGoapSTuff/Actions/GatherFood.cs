using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherFood : Action
{
    private bool collectedFood = false; // do we have food already on us
    private Berrybush targetbush; // where to get food from

    private float startTime = 0;
    public float workDuration = 2;//time it takes to collect food

    public GatherFood()
    {
        //addPrecondition("hasFood", false); // if we have food, why are we collecting
        addEffect("hasFood", true); // we now have food
		addEffect("collectedFood", true);
    }
    public override void reset()
    {
		collectedFood = false;
        targetbush = null;
        startTime = 0;
    }
    public override bool isDone()
    {
        return collectedFood;
    }
    public override bool requiresInRange()
    {
        return true; // yes we need to be near a bush duh
    }
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		// find the nearest tree that we can chop
		Berrybush[] bushes = (Berrybush[])UnityEngine.GameObject.FindObjectsOfType(typeof(Berrybush));
		Berrybush closest = null;
		float closestDist = 0;

		foreach (Berrybush bush in bushes)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				closest = bush;
				closestDist = (bush.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (bush.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist)
				{
					// we found a closer one, use it
					closest = bush;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetbush = closest;
		target = targetbush.gameObject;

		return closest != null;
	}
	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > workDuration)
		{
			// finished chopping
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			backpack.food += 1;
			collectedFood = true;
		}
		return true;
	}
}
