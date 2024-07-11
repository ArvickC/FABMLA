using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

/// <summary>
/// Agent-Based Modeling Agent Class. Used to model a single agent within a system. 
/// </summary> 
public class ABMAgent : Agent {
    protected ABMController controller;
    protected ActionBuffers actions;

    private bool readyToStep = false;

    /// <summary>
    ///  Called when initializing the agent, when the object gets created for the first time.
    /// </summary>
    public override void Initialize() {
        controller = FindFirstObjectByType<ABMController>(); // Get Controller
        controller.RegisterAgent(this);
    }

    /// <summary>
    /// Stores actions made by the Agent to Step() later.
    /// </summary>
    /// <param name="actions">Action decisions made by the Agent Brain</param>
    public override void OnActionReceived(ActionBuffers actions) {
        this.actions = actions; // Store actions
        readyToStep = true;
        controller.AgentReady();
    }

    /// <summary>
    /// Applys the saved actions of the Agent.
    /// </summary>
    public virtual void Step() { readyToStep = false; } // Apply actions in child

    /// Getters
    public bool IsReadyToStep() { return readyToStep; }
    public ABMController GetController() { return controller; }
}