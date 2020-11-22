using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffSticks : Action
{
	public bool hasDroppedOffSticks = false;
	public StoragePile targetStorage;

	private float startTime = 0;
	public float workDuration = 1;

	public DropOffSticks()
	{
		addPrecondition("hasSticks", true);
		//addEffect("hasFood", false);
		addEffect("hasDroppedOffSticks", true);
	}
	public override void reset()
	{
		hasDroppedOffSticks = false;
		targetStorage = null;
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
		// find the nearest tree that we can chop
		StoragePile[] storages = (StoragePile[])UnityEngine.GameObject.FindObjectsOfType(typeof(StoragePile));
		StoragePile closest = null;
		float closestDist = 0;

		foreach (StoragePile storage in storages)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				closest = storage;
				closestDist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist)
				{
					// we found a closer one, use it
					closest = storage;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetStorage = closest;
		target = targetStorage.gameObject;

		return closest != null;
	}

	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		//wait for job to be seen as done
		if (Time.time - startTime > workDuration)
		{
			//finished dropping off
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			//add sticks to storage
			target.GetComponent<StoragePile>().numSticks += backpack.wood;
			//get rid of sticks from backpack
			backpack.wood -= backpack.wood;
			
			hasDroppedOffSticks = true;
		}
		return true;
	}
}
