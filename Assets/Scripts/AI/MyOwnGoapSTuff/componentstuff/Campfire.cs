using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour 
{
    public float fireWood = 100.0f;
    public bool hasFireWood = true;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<WorldState>().worldstatus.Add("hasFireWood", true);
        //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("hasFireWood", true);
        //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isNextToHeater", false);
        //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isResting", false);
    }

    // Update is called once per frame
    void Update()
    {
        fireWood -= Time.deltaTime;
        if (fireWood <= 0)
        {
            fireWood = 0;
            hasFireWood = false;
            //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("hasFireWood");
           // GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("hasFireWood", false);
        }
        else
        {
            hasFireWood = true;
            //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("hasFireWood");
            //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("hasFireWood", true);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
        
    //    if(other.GetComponent<Backpack>().wood > 0 || fireWood > 0)
    //    {
    //        other.GetComponent<AiNeeds>().isNextToHeater = true;
    //        //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("isNextToHeater");
    //        //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isNextToHeater", true) ;
    //        fireWood += other.GetComponent<Backpack>().wood;
    //        other.GetComponent<Backpack>().wood = 0;
    //    }
    //    other.GetComponent<AiNeeds>().isResting = true;
    //    //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("isResting");
    //    //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isResting", true);

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    other.GetComponent<AiNeeds>().isNextToHeater = false;
    //    //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("isNextToHeater");
    //    //.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isNextToHeater", false);
    //    other.GetComponent<AiNeeds>().isResting = false;
    //    //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Remove("isResting");
    //    //GameObject.Find("Ground").GetComponent<WorldState>().worldstatus.Add("isResting", false);
    //    //other.GetComponent<MyFSM>().isDoingSomething = false;
    //}
}
