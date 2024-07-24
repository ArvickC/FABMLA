using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CoopPuzzleAgent : ABMAgent {
    public override int vectorObservationSpaceSize => 18;
    public override int vectorStackSize => 1;
    public override string behaviorName => "CoopPuzzle";
    public float turnSpeed = 20f;
    public bool onOnePlate = false;

    private CoopPuzzleController c;
    private Rigidbody rb;

    public override void Initialize() {
        base.Initialize();
        c = (CoopPuzzleController)controller;
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin() {
        rb.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(this.transform.localPosition); // 3
        sensor.AddObservation(this.transform.localRotation); // 4
        sensor.AddObservation(transform.forward); // 3
        sensor.AddObservation(c.plate.transform.localPosition); // 3
        sensor.AddObservation(c.target.transform.localPosition); // 3
        
        bool onPlate = c.agentsOnPlate.Count > 0;
        bool onTarget = c.agentsOnTarget.Count > 0;
        sensor.AddObservation(onPlate); // 1
        sensor.AddObservation(onTarget); // 1
    }

    public override void Step() {
        base.Step();

        float forward = actions.ContinuousActions[0];
        float rotation = actions.ContinuousActions[1];
        rb.velocity = transform.forward * forward * 2f;
        transform.Rotate(Vector3.up * rotation * turnSpeed * Time.deltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var con = actionsOut.ContinuousActions;
        con[0] = Input.GetAxis("Vertical");
        con[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Plate")) {
            c.agentsOnPlate.Add(this.GetInstanceID());
            onOnePlate = true;
        } else if(other.CompareTag("Target")) {
            c.agentsOnTarget.Add(this.GetInstanceID());
            onOnePlate = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Plate")) {
            c.agentsOnPlate.Remove(this.GetInstanceID());
            onOnePlate = false;
        } else if(other.CompareTag("Target")) {
            c.agentsOnTarget.Remove(this.GetInstanceID());
            onOnePlate = false;
        }
    }
}
