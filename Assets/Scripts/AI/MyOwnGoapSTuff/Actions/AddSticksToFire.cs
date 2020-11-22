using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSticksToFire : Action
{
	public bool hasDroppedOffSticks = false;
	public Campfire targetFire;

	private float startTime = 0;
	public float workDuration = 1;

	public AddSticksToFire()
	{
		addPrecondition("hasSticks", true);
		//addEffect("hasSticks", false);
		addEffect("campFireHasWood", true);
	}
	public override void reset()
	{
		hasDroppedOffSticks = false;
		targetFire = null;
		startTime = 0;
	}
	public override bool isDone()
	{
		return hasDroppedOffSticks;
	}
	public override bool requiresInRange()
	{
		return true;
	}
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		
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

		targetFire = closest;
		target = targetFire.gameObject;

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
			//add sticks to fire so yaknow it actually is lit... ai was warming up from ashes before.. 
			target.GetComponent<Campfire>().fireWood += backpack.wood;
			//drop off all wood they are carrying
			backpack.wood -= backpack.wood;
			hasDroppedOffSticks = true;
		}
		return true;
	}
}
