using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNodeStuff : MonoBehaviour
{
 
        public MyNodeStuff[] neighbors;
        public List<MyNodeStuff> history;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    public static List<MyNodeStuff> Breadthwise(MyNodeStuff start, MyNodeStuff end)
    {
        List<MyNodeStuff> result = new List<MyNodeStuff>();
        List<MyNodeStuff> visited = new List<MyNodeStuff>();
        Queue<MyNodeStuff> work = new Queue<MyNodeStuff>();

        start.history = new List<MyNodeStuff>();
        visited.Add(start);
        work.Enqueue(start);

        while (work.Count > 0)
        {
            MyNodeStuff current = work.Dequeue();
            if (current == end)
            {
                //Found Node
                result = current.history;
                result.Add(current);
                return result;
            }
            else
            {
                //Didn't find Node
                for (int i = 0; i < current.neighbors.Length; i++)
                {
                    MyNodeStuff currentNeighbor = current.neighbors[i];
                    if (!visited.Contains(currentNeighbor))
                    {
                        currentNeighbor.history = new List<MyNodeStuff>(current.history);
                        currentNeighbor.history.Add(current);
                        visited.Add(currentNeighbor);
                        work.Enqueue(currentNeighbor);
                    }
                }
            }
        }
        //Route not found, loop ends
        return null;
    }

}
