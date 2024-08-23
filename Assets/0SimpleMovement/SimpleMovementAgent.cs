using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class SimpleMovementAgent : ABMAgent {
    public override int vectorObservationSpaceSize => 3;
    public override int vectorStackSize => 1;
    public override String behaviorName => "SimpleMovementAgent";
    public Vector3 initialState = Vector3.zero;
    private Rigidbody rb;

    public override void Initialize() {
        base.Initialize();
        rb = GetComponent<Rigidbody>();
        ResetAgent();
    }

    public override void OnEpisodeBegin() { ResetAgent(); }

    public override void CollectObservations(VectorSensor sensor) { 
        sensor.AddObservation(this.transform.localPosition); // Vector3 -> 3
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
        this.transform.localPosition = initialState;
    }
    
    private void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
