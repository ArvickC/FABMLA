using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PongController : ABMController {
    [Header("Ball")]
    public BallManager ball;
    public float moveSpeed;

    [Header("Blue")]
    //public GameObject blueTarget;
    public Material blue;
    public Vector3 blueSpawn = new Vector3(-16f, 0f, 0f);

    [Header("Purple")]
   //public GameObject purpleTarget;
    public Material purple;
    public Vector3 purpleSpawn = new Vector3(-16f, 0f, 0f);

    [Header("Agents")]
    public float step;
    
    private PongAgent blueAgent;
    private PongAgent purpleAgent;
    [HideInInspector] public bool scored = false;

    protected override void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            if(isEnd) return;

            if(a.MaxStep >= 1 && a.StepCount >= a.MaxStep) { isEnd = true; EndAll(); } // Stepcount reached
            if(scored) { isEnd = true; EndAll(); }
        });
    }

    protected override void Start() {
        Spawn(blue, blueSpawn, out blueAgent);
        SetTeamId(blueAgent, 0);

        Spawn(purple, purpleSpawn, out purpleAgent);
        SetTeamId(purpleAgent, 1);

        SetSpawn(ball.gameObject, Vector3.zero);
        ball.moveSpeed = moveSpeed;

        blueAgent.step = step;
        purpleAgent.step = step;

        scored = false;
    }

    protected override void SetupEpisode() {
        SetSpawn(blueAgent.gameObject, blueSpawn);
        SetSpawn(purpleAgent.gameObject, purpleSpawn);

        SetSpawn(ball.gameObject, Vector3.zero);
        ball.Reset();

        scored = false;
    }

    public void Score(float blueReward, float purpleReward) {
        blueAgent.AddReward(blueReward);
        purpleAgent.AddReward(purpleReward);
    }

    private void Spawn(Material m, Vector3 spawn, out PongAgent agent) {
        GameObject a = Instantiate(simulationAgent);
        a.GetComponent<MeshRenderer>().material = m;
        SetSpawn(a, spawn);
        agent = a.GetComponent<PongAgent>();
    }

    private void SetSpawn(GameObject a, Vector3 spawn) {
        a.transform.localPosition = spawn;
    }

    protected override void CalculateReward(ABMAgent agent) {
        agent.AddReward(-0.001f);
    }

    public ABMAgent GetBlueAgent() { return blueAgent; }
    public ABMAgent GetPurpleAgent() { return purpleAgent; }
    public Vector3 GetBallLocation() { return ball.transform.localPosition; }
    public Vector3 GetBallVelocity() { return ball.GetVelocty(); }
}
