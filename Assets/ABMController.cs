using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class ABMController : MonoBehaviour
{
    protected List<ABMAgent> agents = new List<ABMAgent>();
    private bool allAgentsReady = false;
    public GameObject simulationAgent;
    public int agentAmount = 1;

    void Start() {
        for(int i=0;i<agentAmount;i++) {
            GameObject a = Instantiate(simulationAgent); // Make agents
        }
    }

    public void RegisterAgent(ABMAgent agent) {
        agents.Add(agent);
    }

    public void AgentReady() {
        allAgentsReady = agents.TrueForAll(a => a.IsReadyToStep());
        if(allAgentsReady) {
            agents.ForEach(a => a.Step());
            EndCase(); // Should we end episode?
        }
    }

    protected void EndAll() { // End episode for all agents
        agents.ForEach(a => a.EndEpisode());
        SetupEpisode();
    }

    protected virtual void EndCase() {
        bool isEnd = false;
        agents.ForEach(a => {
            if(isEnd) return;
            if(a.StepCount >= a.MaxStep) {isEnd = true; EndAll();}
        });
    }
    protected virtual void CalculateReward(ABMAgent agent) {}
    protected virtual void SetupEpisode() {}
}