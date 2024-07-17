using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Pong2DAgent : ABMAgent {
    public override int vectorObservationSpaceSize => 6;
    public override int vectorStackSize => 1;
    public override string behaviorName => "Pong2D";
    public float speed = 5f;

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition); // 3
        sensor.AddObservation(((Pong2DContorller)controller).ball.transform.localPosition); // 3
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
    }

    public override void Step() {
        base.Step();
        float y = actions.ContinuousActions[0];
        transform.localPosition += new Vector3(0, y, 0) * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Target")) {
            AddReward(1f);
        }
    }

    public void SetTeam(Team team) {
        if(team == Team.Blue) {
            GetComponent<BehaviorParameters>().TeamId = 0;
            GetComponent<MeshRenderer>().material = ((Pong2DContorller)controller).m[1];
        } else if(team == Team.Purple) {
            GetComponent<BehaviorParameters>().TeamId = 1;
            GetComponent<MeshRenderer>().material = ((Pong2DContorller)controller).m[2];
        }
    }
}