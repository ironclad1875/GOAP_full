using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MyGOAP
{
    HashSet<KeyValuePair<string, object>> getWorldState();

    HashSet<KeyValuePair<string, object>> createGoalState();

    void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<Action> actions);

    void actionsFinished();

    void planFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    void planAborted(Action aborter);

    bool moveAgent(Action nextAction);



}
