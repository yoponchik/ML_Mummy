#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class MummyAgent : Agent
{
    /*
        1. Observation
        2. Action according to Policy
        3. Reward 
     */

    private Transform tr;
    private Transform targetTr;
    private Rigidbody rb;

    public Material goodMt, badMt;
    private Material originalMt;
    private new Renderer renderer;

    public override void Initialize() {

        tr = GetComponent<Transform>();
        targetTr = tr.parent.Find("Target").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        //extracts the MeshRenderer Component from the game object floor
        renderer = tr.parent.Find("Floor").GetComponent<Renderer>();
        originalMt = renderer.material;
    }

    //Called everytime the episode (every learning cycle?¥‹¿ß) begins
    public override void OnEpisodeBegin()
    {
        //reset physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        //make agent position random
        tr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f), 0.05f, Random.Range(-4.0f, 4.0f));

        //make target position random
        targetTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f), 0.05f, Random.Range(-4.0f, 4.0f));

    }

    //Method that observes with sensor which feeds data to the gameobject
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.localPosition); //x,y,z 3
        sensor.AddObservation(tr.localPosition); //x,y,z 3
        sensor.AddObservation(rb.velocity.x); //x 1
        sensor.AddObservation(rb.velocity.z); //z 1
        //8 in all
    }

    //Carries out actions according to the policy 
    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.ContinuousActions;
        //Debug.Log($"[0]={action[0]}, [1]={action[1]}");
        //Debug.Log("[0]=" + action[0] + ", [1]" + action[1]); //same thing

        Vector3 dir = (Vector3.forward * action[0] + Vector3.right * action[1]);
        rb.AddForce(dir.normalized * 50.0f);

        //makes agent prefer continuous motion due to minus penalty
        SetReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //2 types of data 1. continuous 2. discrete

        var actions = actionsOut.ContinuousActions;
        //updown
        actions[0] = Input.GetAxis("Vertical");
        //leftright
        actions[1] = Input.GetAxis("Horizontal");

    }

    private void OnCollisionEnter(Collision other)
    {
        //gives penalty when hits the wall
        if (other.collider.CompareTag("DEAD_ZONE")) {
            StartCoroutine(RevertMaterial(badMt));
            SetReward(-1.0f);
            EndEpisode();
        }

        //gives reward when touches the target
        if (other.collider.CompareTag("TARGET")) {
            StartCoroutine(RevertMaterial(goodMt));
            SetReward(+1.0f);
            EndEpisode();
        }
    }

    IEnumerator RevertMaterial(Material changedMt) {
        renderer.material = changedMt;
        yield return new WaitForSeconds(0.2f);
        renderer.material = originalMt;
    }
}
