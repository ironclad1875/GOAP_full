using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFood : Action
{
	private bool hasFood = false; // do we have food already on us
	private StoragePile targetstorage; // where to get food from

	private float startTime = 0;
	public float workDuration = 1;//time it takes to collect food

	public PickUpFood()
	{
		addPrecondition("isHungry", true); // they should be hungry before picking up from strage.. this stops them from getting stuck picking up and putting back down endlessly 
		// side note, AI dumb
		addEffect("hasFood", true); // we now have food

	}
	public override void reset()
	{
		hasFood = false;
		targetstorage = null;
		startTime = 0;
	}
	public override bool isDone()
	{
		return hasFood;
	}
	public override bool requiresInRange()
	{
		return true; // yes we need to be near a bush duh
	}
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		//nearest storage
		StoragePile[] storages = (StoragePile[])UnityEngine.GameObject.FindObjectsOfType(typeof(StoragePile));
		StoragePile closest = null;
		float closestDist = 0;

		foreach (StoragePile storage in storages)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				if(storage.numfood > 0)
				{
					closest = storage;
					closestDist = (storage.gameObject.transform.position - agent.transform.position).magnitude;
				}
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

		targetstorage = closest;
		target = targetstorage.gameObject;

		return closest != null;
	}
	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;
		//wait for time before job is seen as done
		if (Time.time - startTime > workDuration)
		{
			//add food to the backpack after job is done
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			backpack.food += 1;
			hasFood = true;
		}
		return true;
	}
}
