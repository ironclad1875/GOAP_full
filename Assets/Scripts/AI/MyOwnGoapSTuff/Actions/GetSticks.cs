using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSticks : Action
{
	private bool hasPickedUpSticks = false;
	private StoragePile targetpile;

	private float startTime = 0;
	public float workDuration = 2;
	public GetSticks()
	{
		addPrecondition("isCold", true); //if cold can get sticks from the storage
		addEffect("hasSticks", true); // now has sticks
		addEffect("hasPickedUpSticks", true); // just a bool showing action has been done
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
		// find the nearest supply pile that has spare firewood
		StoragePile[] storagepiles = (StoragePile[])UnityEngine.GameObject.FindObjectsOfType(typeof(StoragePile));
		StoragePile closest = null;
		float closestDist = 0;

		foreach (StoragePile storage in storagepiles)
		{
			if (closest == null)
			{
				//making sure that the pile actually has sticks in it.. lets hope this method works
				if (storage.numSticks > 0)
				{
					// first one, so choose it for now
					closest = storage;
					closestDist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
				}
				//if it doesnt return a false to path finding
				else
				{
					return false;
				}

				
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

		targetpile = closest;
		target = targetpile.gameObject;

		return closest != null;
	}

	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		//add wood to backpack so now holding sticks
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
