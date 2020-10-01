using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    float interval = 3;
    float lastHearbeatTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Main Scene Loaded");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Time.time - lastHearbeatTime > interval)
        {
            lastHearbeatTime = Time.time;
            Debug.Log("DEBUGG: SCENE HEARTBEAT");
        }
        */
    }
}
