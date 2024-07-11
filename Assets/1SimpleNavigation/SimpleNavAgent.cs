using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SimpleNavAgent : ABMAgent {
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
        this.transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
