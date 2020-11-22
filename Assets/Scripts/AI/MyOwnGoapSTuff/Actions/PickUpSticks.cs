using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSticks : Action
{
    private bool hasPickedUpSticks = false;
    private Stickpile targetpile;

	private float startTime = 0;
	public float workDuration = 2;
	public PickUpSticks()
    {
        //addPrecondition("hasSticks", false);
        addEffect("hasSticks", true); // now has sticks
        addEffect("hasPickedUpSticks", true);
    }

    public override void reset()
    {
        hasPickedUpSticks = false;
        targetpile = null;
		startTime = 0;
    }
    public override bool isDone()
    {
        return hasPickedUpSticks;
    }
    public override bool requiresInRange()
    {
        return true;
    }
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		
		Stickpile[] stickpiles = (Stickpile[])UnityEngine.GameObject.FindObjectsOfType(typeof(Stickpile));
		Stickpile closest = null;
		float closestDist = 0;

		foreach (Stickpile sticks in stickpiles)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				closest = sticks;
				closestDist = (sticks.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (sticks.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist)
				{
					// we found a closer one, use it
					closest = sticks;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetpile = closest;
		target = targetpile.gameObject;

		return closest != null;
	}

	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > workDuration)
		{
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			backpack.wood += 1;
			hasPickedUpSticks = true;
		}
			
		//backpack.wood = 0;

		return true;
	}
}
