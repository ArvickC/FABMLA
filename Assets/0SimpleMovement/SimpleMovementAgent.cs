using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SimpleMovementAgent : ABMAgent {
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

    public override void Step() {
        base.Step();
        
        Vector3 controlSignal = Vector3.zero;
        // 3 continuous actions
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.y = actions.ContinuousActions[1];
        controlSignal.z = actions.ContinuousActions[2];
        rb.AddForce(controlSignal * 10f);
    }

    private void ResetAgent() {
        this.transform.localPosition = initialState;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
