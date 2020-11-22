using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
	

	private MyFSM stateMachine;

	private MyFSM.WorldState idleState; // finds something to do
	private MyFSM.WorldState moveToState; // moves to a target
	private MyFSM.WorldState performActionState; // performs an action

	private HashSet<Action> availableActions;
	private Queue<Action> currentActions;

	private MyGOAP dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning

	private planner planner;


	void Start()
	{
		stateMachine = new MyFSM();
		availableActions = new HashSet<Action>();
		currentActions = new Queue<Action>();
		planner = new planner();
		findDataProvider();
		createIdleState();
		createMoveToState();
		createPerformActionState();
		stateMachine.pushState(idleState);
		loadActions();
	}


	void Update()
	{
		stateMachine.Update(this.gameObject);
	}


	public void addAction(Action a)
	{
		availableActions.Add(a);
	}

	public Action getAction(Type action)
	{
		foreach (Action g in availableActions)
		{
			if (g.GetType().Equals(action))
				return g;
		}
		return null;
	}

	public void removeAction(Action action)
	{
		availableActions.Remove(action);
	}

	private bool hasActionPlan()
	{
		return currentActions.Count > 0;
	}

	private void createIdleState()
	{
		idleState = (fsm, gameObj) => {
			// GOAP planning

			// get the world state and the goal we want to plan for
			HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState();
			HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState();

			// Plan
			Queue<Action> plan = planner.plan(gameObject, availableActions, worldState, goal);
			if (plan != null)
			{
				// we have a plan, hooray!
				currentActions = plan;
				dataProvider.planFound(goal, plan);

				fsm.popState(); // move to PerformAction state
				fsm.pushState(performActionState);

			}
			else
			{
				// ugh, we couldn't get a plan
				Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
				dataProvider.planFailed(goal);
				fsm.popState(); // move back to IdleAction state
				fsm.pushState(idleState);
			}

		};
	}

	private void createMoveToState()
	{
		moveToState = (fsm, gameObj) => {
			// move the game object

			Action action = currentActions.Peek();
			if (action.requiresInRange() && action.target == null)
			{
				Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
				fsm.popState(); // move
				fsm.popState(); // perform
				fsm.pushState(idleState);
				return;
			}

			// get the agent to move itself
			if (dataProvider.moveAgent(action))
			{
				fsm.popState();
			}

			
		};
	}

	private void createPerformActionState()
	{

		performActionState = (fsm, gameObj) => {
			// perform the action

			if (!hasActionPlan())
			{
				// no actions to perform
				Debug.Log("<color=red>Done actions</color>");
				fsm.popState();
				fsm.pushState(idleState);
				dataProvider.actionsFinished();
				return;
			}

			Action action = currentActions.Peek();
			if (action.isDone())
			{
				// the action is done. Remove it so we can perform the next one
				currentActions.Dequeue();
			}

			if (hasActionPlan())
			{
				// perform the next action
				action = currentActions.Peek();
				bool inRange = action.requiresInRange() ? action.isInRange() : true;

				if (inRange)
				{
					// we are in range, so perform the action
					bool success = action.perform(gameObj);

					if (!success)
					{
						// action failed, we need to plan again
						fsm.popState();
						fsm.pushState(idleState);
						dataProvider.planAborted(action);
					}
				}
				else
				{
					// we need to move there first
					// push moveTo state
					fsm.pushState(moveToState);
				}

			}
			else
			{
				// no actions left, move to Plan state
				fsm.popState();
				fsm.pushState(idleState);
				dataProvider.actionsFinished();
			}

		};
	}

	private void findDataProvider()
	{
		foreach (Component comp in gameObject.GetComponents(typeof(Component)))
		{
			if (typeof(MyGOAP).IsAssignableFrom(comp.GetType()))
			{
				dataProvider = (MyGOAP)comp;
				return;
			}
		}
	}

	private void loadActions()
	{
		Action[] actions = gameObject.GetComponents<Action>();
		foreach (Action a in actions)
		{
			availableActions.Add(a);
		}
		Debug.Log("Found actions: " + prettyPrint(actions));
	}

	public static string prettyPrint(HashSet<KeyValuePair<string, object>> state)
	{
		String s = "";
		foreach (KeyValuePair<string, object> kvp in state)
		{
			s += kvp.Key + ":" + kvp.Value.ToString();
			s += ", ";
		}
		return s;
	}

	public static string prettyPrint(Queue<Action> actions)
	{
		String s = "";
		foreach (Action a in actions)
		{
			s += a.GetType().Name;
			s += "-> ";
		}
		s += "GOAL";
		return s;
	}

	public static string prettyPrint(Action[] actions)
	{
		String s = "";
		foreach (Action a in actions)
		{
			s += a.GetType().Name;
			s += ", ";
		}
		return s;
	}

	public static string prettyPrint(Action action)
	{
		String s = "" + action.GetType().Name;
		return s;
	}
}
