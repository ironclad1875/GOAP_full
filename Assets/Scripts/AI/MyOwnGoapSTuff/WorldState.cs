using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WorldState
{
    //public Hashtable worldstatus = new Hashtable();
    //public int i = 0;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    while (i < 5)
    //    {
    //        Debug.Log( (string) worldstatus[i]);
    //        i++;
    //    }


    //}
    void Update(MyFSM fsm, GameObject gameObject);


}
