using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Policies;
using UnityEngine;

public class BallManager : MonoBehaviour {
    public float moveSpeed;
    private Rigidbody rb;
    private PongController controller;

    void Start() {
        rb = GetComponent<Rigidbody>();
        controller = FindFirstObjectByType<PongController>();
        Reset();
    }

    public void Reset() {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(RandomDirection(), RandomDirection(), 0f) 
                    * moveSpeed);
    }

    private int RandomDirection() {
        int x = UnityEngine.Random.Range(0, 2);
        if(x == 0) return -1;
        else return 1;
    }

    public Vector3 GetVelocty() { return rb.velocity; }

    private void OnTriggerEnter(Collider other) { // Score
        TargetManager tm;
        if(!other.gameObject.TryGetComponent<TargetManager>(out tm)) return;

        if(tm.team == TargetManager.Teams.Blue) { // Blue
            controller.Score(1f, -1f);
        } else { // Purple
            controller.Score(-1f, 1f);
        }

        controller.scored = true;
    }
}