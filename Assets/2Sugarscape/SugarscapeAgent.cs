using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SugarscapeAgent : ABMAgent {
    public int sugar;
    [HideInInspector] public float id;
    private int metabolism;

    public override void Initialize() {
        base.Initialize();
        id = UnityEngine.Random.Range(0f, 100f);
        ResetAgent();
    }

    public override void CollectObservations(VectorSensor sensor) { // Total -> 3 + 1 + 1 + 1 + 1 + 1 = 8
        SugarscapeController c = controller.GetComponent<SugarscapeController>();

        sensor.AddObservation(this.transform.localPosition); // Vector3 -> 3
        sensor.AddObservation(sugar); // int -> 1

        sensor.AddObservation(GetObservation(Vector3.left, c)); // int -> 1
        sensor.AddObservation(GetObservation(Vector3.right, c)); // int -> 1
        sensor.AddObservation(GetObservation(Vector3.down, c)); // int -> 1
        sensor.AddObservation(GetObservation(Vector3.up, c)); // int -> 1
    }

    // Get value of sugar cube in dir direction
    private int GetObservation(Vector3 dir, SugarscapeController c) {
        if(dir == Vector3.left) { // LEFT
            if(CheckBounds(-1, 0)) {
                return c.grid[transform.localPosition + Vector3.left].value;
            } else return 0;
        } else if(dir == Vector3.right) { // RIGHT
            if(CheckBounds(1, 0)) {
                return c.grid[transform.localPosition + Vector3.right].value;
            } else return 0;
        } else if(dir == Vector3.down) { // DOWN
            if(CheckBounds(0, -1)) {
                return c.grid[transform.localPosition + Vector3.down].value;
            } else return 0;
        } else if(dir == Vector3.up) { // UP
            if(CheckBounds(0, 1)) {
                return c.grid[transform.localPosition + Vector3.up].value;
            } else return 0;
        }
        return 0;
    }

    private bool CheckBounds(int x, int y) {
        return (Math.Abs(transform.localPosition.x + x) <= 8 && Math.Abs(transform.localPosition.y + y) <= 8);
    }

    public override void Step() {
        base.Step();

        int a = actions.DiscreteActions[0];

        if(a == 0) this.transform.localPosition += Vector3.up;
        if(a == 1) this.transform.localPosition += Vector3.down;
        if(a == 2) this.transform.localPosition += Vector3.left;
        if(a == 3) this.transform.localPosition += Vector3.right;

        sugar -= metabolism;
    }


    private void OnTriggerEnter(Collider c) { // Hit another agent
        if(!c.gameObject.CompareTag("Agent")) return;
        if(sugar < c.gameObject.GetComponent<SugarscapeAgent>().id) { controller.ClearAgent(this); }
        if(id < c.gameObject.GetComponent<SugarscapeAgent>().id) { controller.ClearAgent(this); }
        controller.CheckToEndEpisode();
    }

    private void ResetAgent() {
        this.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-controller.GetComponent<SugarscapeController>().radius, controller.GetComponent<SugarscapeController>().radius),
            UnityEngine.Random.Range(-controller.GetComponent<SugarscapeController>().radius, controller.GetComponent<SugarscapeController>().radius),
            0); // Random placement
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        sugar = UnityEngine.Random.Range(5, 7);
        metabolism = UnityEngine.Random.Range(1, 3);
    }
}
