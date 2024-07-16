using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using System;

/// <summary>
/// Agent-Based Modeling Agent Class. Used to model a single agent within a system. 
/// </summary> 
public abstract class ABMAgent : Agent {
    [Header("Params")]
    abstract public int vectorObservationSpaceSize { get; }
    abstract public int vectorStackSize { get; }
    abstract public String behaviorName { get; }

    protected ABMController controller;
    protected ActionBuffers actions;

    private bool readyToStep = false;

    /// <summary>
    ///  Called when initializing the agent, when the object gets created for the first time.
    /// </summary>
    public override void Initialize() {
        controller = FindFirstObjectByType<ABMController>(); // Get Controller
        controller.RegisterAgent(this);

        GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = vectorObservationSpaceSize;
        GetComponent<BehaviorParameters>().BrainParameters.NumStackedVectorObservations = vectorStackSize;
        GetComponent<BehaviorParameters>().BehaviorName = behaviorName;
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