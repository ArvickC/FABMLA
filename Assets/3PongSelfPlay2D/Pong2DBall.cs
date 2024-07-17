using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong2DBall : MonoBehaviour {
    public float speed = 5f;
    public Pong2DContorller controller;

    private Rigidbody rb;
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch() {
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector3(speed * x, speed * y);
    }

    private void OnTriggerEnter(Collider o) {
        if(o.CompareTag("BlueGoal")) {
            controller.Score(Team.Purple, o.gameObject); // Purple = 1
        } else if(o.CompareTag("PurpleGoal")) {
            controller.Score(Team.Blue, o.gameObject); // Blue = 0
        }
    }
}