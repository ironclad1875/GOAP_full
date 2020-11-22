//using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : Action
{
    private bool hasEaten = false;
	private Character tragetCharacter;
    private float startTime = 0;
    public float workTime = 1;

    public Eat()
    {
        addPrecondition("hasFood", true);
        //addPrecondition("isHungry", true);
		//addEffect("isHungry", false);
		addEffect("hasEaten", true);
    }
    public override void reset()
    {
        hasEaten = false;
        startTime = 0;
    }
    public override bool isDone()
    {
        return hasEaten;
    }
    public override bool requiresInRange()
    {
        return false; //can eat anywhere, might add a place for eating but no need right now, this isnt dwarffortress
    }
	//all this is un-needed, i think at least, will just comment out for the moment // so it was needed.. for some reason, though 
	// the requires in range bool above negates this bellow.. but the code has a fit if its not in so just left in the base code unchanged.
	public override bool checkProceduralPrecondition(GameObject agent)
	{
		// find the nearest tree that we can chop
		Character[] trees = (Character[])UnityEngine.GameObject.FindObjectsOfType(typeof(Character));
		Character closest = null;
		float closestDist = 0;

		foreach (Character tree in trees)
		{
			if (closest == null)
			{
				// first one, so choose it for now
				closest = tree;
				closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
			}
			else
			{
				// is this one closer than the last?
				float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist)
				{
					// we found a closer one, use it
					closest = tree;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		tragetCharacter = closest;
		target = tragetCharacter.gameObject;

		return closest != null;
	}
	public override bool perform(GameObject agent)
	{
		if (startTime == 0)
			startTime = Time.time;

		if (Time.time - startTime > workTime)
		{
			// finished chopping
			Backpack backpack = (Backpack)agent.GetComponent(typeof(Backpack));
			Character need = (Character)agent.GetComponent(typeof(Character));
			int numfood = backpack.food;
			need.hunger += (10 * numfood);
			backpack.food = 0;
			hasEaten = true;
			
		}
		return true;
	}

}
