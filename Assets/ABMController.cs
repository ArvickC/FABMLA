using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Linq;

/// <summary>
/// Agent-Based Modeling Controller Class. Used to control the system. It synchronises the Step()s of the Agents
/// </summary> 
public class ABMController : MonoBehaviour {
    protected List<ABMAgent> agents = new List<ABMAgent>(); // List of all agents in scene

    private bool allAgentsReady = false;

    public GameObject simulationAgent; // Reference to Agent Prefab
    public int agentAmount = 1;

    /// <summary>
    /// Built-in Unity3D method. Called on scene setup. Reference Unity3D documentation for more information
    /// </summary>
    protected virtual void Start() { SetupEpisode(); }

    /// <summary>
    /// Registers an Agent into the controller
    /// </summary>
    /// <param name="agent">The Agent to register</param>
    public void RegisterAgent(ABMAgent agent) { agents.Add(agent); }
    
    /// <summary>
    /// Step()s all agents if they are ready
    /// </summary>
    public void AgentReady() {
        allAgentsReady = agents.TrueForAll(a => a.IsReadyToStep());

        if(allAgentsReady) {
            agents.ToList().ForEach(a => { a.Step(); CalculateReward(a); });
            EndCase(); // Should we end episode?
        }
    }

    /// <summary>
    /// Ends the episode for all agents
    /// </summary>
    protected void EndAll() { // End episode for all agents
        if(agents.Count <= 0) return;
        agents.ToList().ForEach(a => a.EndEpisode());
        SetupEpisode();
    }

    /// <summary>
    /// The conditions of and what happens when the episode ends
    /// </summary>
    protected virtual void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            if(isEnd) return;
            if(a.MaxStep > 0 && a.StepCount >= a.MaxStep) { isEnd = true; EndAll(); } // Max stepcount reached
        });
    }

    /// <summary>
    /// Calculates the reward of each Agent.
    /// </summary>
    /// <param name="agent">The Agent to calculate the reward for</param>
    protected virtual void CalculateReward(ABMAgent agent) {}

    /// <summary>
    /// Sets up the current episode
    /// </summary>
    protected virtual void SetupEpisode() {
        for(int i=0;i<agentAmount;i++) {
            GameObject a = Instantiate(simulationAgent); // Make Agents
        }
    }

    /// <summary>
    /// Remove all Agents from a scene
    /// </summary>
    protected void ClearAgents() { agents.ToList().ForEach(a => ClearAgent(a)); }

    /// <summary>
    /// Remove a specific Agent from a scene
    /// </summary>
    /// <param name="a">Agent to remove</param>
    public void ClearAgent(ABMAgent a) {
        agents.Remove(a);
        Destroy(a.gameObject);
    }
}