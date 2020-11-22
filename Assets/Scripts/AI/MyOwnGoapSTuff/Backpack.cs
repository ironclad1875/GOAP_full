using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public int wood = 0;
    public int food = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(food > 0)
        {
            gameObject.GetComponent<AiNeeds>().hasFood = true;
        }
        else
        {
            gameObject.GetComponent<AiNeeds>().hasFood = false;
        }
    }
}
