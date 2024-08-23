using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SimpleNavAgent : ABMAgent {
    public override int vectorObservationSpaceSize => 6;
    public override int vectorStackSize => 1;
    public override String behaviorName => "SimpleNavAgent";
    public float boundsRadius = 5f;
    private Rigidbody rb;

    public override void Initialize() {
        base.Initialize();
        rb = this.GetComponent<Rigidbody>();
        ResetAgent();
    }

    public override void OnEpisodeBegin() { ResetAgent(); }

    public override void CollectObservations(VectorSensor sensor) { // Total -> 3 + 3 = 6
        sensor.AddObservation(this.transform.localPosition); // Vector3 -> 3
        sensor.AddObservation(controller.GetComponent<SimpleNavController>().target.transform.localPosition); // Vector3 -> 3
    }

    public override void Step()
    {
        base.Step();
        ApplyControlSignal();
    }
    
    private void ApplyControlSignal()
    {
        Vector3 controlSignal = new Vector3(
            actions.ContinuousActions[0],
            actions.ContinuousActions[1],
            actions.ContinuousActions[2]
        );
        rb.AddForce(controlSignal * 10f);
    }
    
    private void ResetAgent()
    {
        ResetPosition();
        ResetVelocity();
    }
    
    private void ResetPosition()
    {
        this.transform.localPosition = Vector3.zero;
    }
    
    private void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
