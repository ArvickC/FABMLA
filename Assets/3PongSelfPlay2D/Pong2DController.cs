using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Policies;
using UnityEngine;

public enum Team {
    Blue, Purple
}

public class Pong2DContorller : ABMController {
    public Material[] m; // White, Blue, Purple

    [Header("Ball")]
    public Pong2DBall ball;
    public float ballSpeed = 5f;

    [Header("Agents")]
    private Pong2DAgent blue;
    private Pong2DAgent purple;
    public float agentSpeed = 5f;
    public int spawn = -8;

    protected override void Start() {
        Spawn(Team.Blue, out blue);
        Spawn(Team.Purple, out purple);
        blue.speed = agentSpeed; purple.speed = agentSpeed;

        ball.speed = ballSpeed;
        ball.controller = this;

        SetupEpisode();
    }

    private void Spawn(Team team, out Pong2DAgent agent) {
        GameObject b = Instantiate(simulationAgent);
        agent = b.GetComponent<Pong2DAgent>();
        agent.SetTeam(team);
    }

    protected override void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            if(isEnd) return;
            if(a.MaxStep > 0 && a.StepCount >= a.MaxStep) { isEnd = true; EndAll(); } // Max stepcount reached
        });
    }

    public void Score(Team team, GameObject goal) {
        if(team == Team.Blue) { // Blue scored
            blue.AddReward(1f);
            purple.AddReward(-1f);
            StartCoroutine(Color(goal, m[2]));
            EndAll();
        } else if(team == Team.Purple) { // Purple scored
            blue.AddReward(-1f);
            purple.AddReward(1f);
            StartCoroutine(Color(goal, m[1]));
            EndAll();
        }
    }

    protected override void CalculateReward(ABMAgent agent) {
        agent.AddReward(-0.001f);
    }

    protected override void SetupEpisode() {
        ball.Launch();
        blue.transform.localPosition = new Vector3(spawn, Random.Range(-3.7f, 2.5f), 0);
        purple.transform.localPosition = new Vector3(-spawn, Random.Range(-3.7f, 2.5f), 0);
    }

    IEnumerator Color(GameObject goal, Material mat) {
        goal.GetComponent<MeshRenderer>().material = m[0];
        yield return new WaitForSeconds(0.5f);
        goal.GetComponent<MeshRenderer>().material = mat;
    }

    // DEBUG
    public void End() {
        EndCase();
    }
}
