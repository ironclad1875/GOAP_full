using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffFood : Action
{
    public bool hasDroppedOffFood = false;
    public StoragePile targetStorage;

    private float startTime = 0;
    public float workDuration = 1;

    public DropOffFood()
    {
        addPrecondition("hasFood", true);// make sure they have somethign to drop off, no magic in the world just yet
        //addEffect("hasFood", false);
		addEffect("droppedOffFood", true);
    }
    public override void reset()
    {
        hasDroppedOffFood = false;
        targetStorage = null;
        startTime = 0;
    }
    public override bool isDone()
    {
        return hasDroppedOffFood;
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

		if (Time.time - startTime > workDuration)
		{
			
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			//add food to the storage
			target.GetComponent<StoragePile>().numfood += backpack.food;
			//drop off all the food they are holding
			backpack.food -= backpack.food;
			hasDroppedOffFood = true;
		}
		return true;
	}
}
