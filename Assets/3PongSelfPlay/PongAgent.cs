using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PongAgent : ABMAgent {
    public float step;

    public override int vectorObservationSpaceSize => 6;
    public override int vectorStackSize => 1;
    public override String behaviorName => "PongAgent";
    public int maxRadius = 8;

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(this.transform.localPosition); // 3
        sensor.AddObservation(((PongController)controller).GetBallLocation()); // 3
        //sensor.AddObservation(((PongController)controller).GetBallVelocity()); // 3
    }

    public override void Step() {
        base.Step();

        int action = actions.DiscreteActions[0]; // 3 Actions
        if(action == 1 && Mathf.Abs(this.transform.localPosition.y) < maxRadius) {
            this.transform.localPosition += Vector3.up * step;
        }
        if(action == 2 && Mathf.Abs(this.transform.localPosition.y) < maxRadius) {
            this.transform.localPosition += Vector3.down * step;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if(Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 1;
        if(Input.GetKey(KeyCode.S)) discreteActionsOut[0] = 2;
    }
}
