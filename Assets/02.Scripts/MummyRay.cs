using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MummyRay : Agent
{

    private Transform tr;
    private Rigidbody rb;
    private StageManager stageManager;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 200.0f;



    public override void Initialize() {
        MaxStep = 50;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        stageManager = tr.parent.GetComponent<StageManager>();

    }

    public override void OnEpisodeBegin() {
        //reset stage
        stageManager.InitStage();
        //change agent position
        tr.localPosition = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
        tr.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

    }


    public override void CollectObservations(VectorSensor sensor) { 
    
    }

    //gets data from python machine learning programs and controls the unity stuff
    public override void OnActionReceived(ActionBuffers actions) { 
    
    }


    //for testing out with our keyboards
    public override void Heuristic(in ActionBuffers actionsOut) { 
    
    }
}
