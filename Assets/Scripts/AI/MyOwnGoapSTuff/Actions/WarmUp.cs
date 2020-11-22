using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarmUp : Action
{
    private bool warmingUp = false; // are we currently warming up
    private Campfire targetfire; // where we go to warm up

	private float startTime = 0;
	public float workDuration = 3;
	public WarmUp()
    {
        //addPrecondition("warmingUp", false); // we need to be next to a fire to warm up
        addPrecondition("campFireHasWood", true); // does the campfire have wood
        addEffect("warmingUp", true);
    }
    public override void reset()
    {
        warmingUp = false;
        targetfire = null;
    }
    public override bool isDone()
    {
        return warmingUp;
    }
	public override bool requiresInRange()
	{
		return true; // yes we need to be near a a fire
	}
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		// find the nearest fire 
		Campfire[] fires = (Campfire[])UnityEngine.GameObject.FindObjectsOfType(typeof(Campfire));
		Campfire closest = null;
		float closestDist = 0;

		foreach (Campfire fire in fires)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				closest = fire;
				closestDist = (fire.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (fire.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist)
				{
					// we found a closer one, use it
					closest = fire;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetfire = closest;
		target = targetfire.gameObject;

		return closest != null;
	}
	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		Character need = (Character)agent.GetComponent(typeof(Character));
		//actually warm the character up faster than they cool down // world is apparntly freezing
		need.warmth += Time.deltaTime * 10;
		if (Time.time - startTime > workDuration)
		{
			//Character need = (Character)agent.GetComponent(typeof(Character));
			//need.warmth += Time.deltaTime * 2;
			warmingUp = true;
		}
			
		
		return true;
	}
}
