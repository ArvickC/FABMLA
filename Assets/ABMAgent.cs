using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ABMAgent : Agent
{
    private ABMController controller;
    private bool readyToStep = false;
    protected ActionBuffers actions;

    public override void Initialize()
    {
        controller = FindFirstObjectByType<ABMController>(); // Get Controller
        controller.RegisterAgent(this);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        this.actions = actions; // Store actions
        readyToStep = true;
        controller.AgentReady();
    }

    public bool IsReadyToStep() {return readyToStep;}
    public virtual void Step() {readyToStep = false;} // Apply actions in child
}