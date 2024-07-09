using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class SimpleMovementController : ABMController
{
    public float boundsRadius = 5f;
    protected override void EndCase() {
        bool isEnd = false;
        agents.ForEach(a => {
            if(isEnd) { return; }

            if(Distance(a)>boundsRadius || a.StepCount >= a.MaxStep) { isEnd = true; EndAll(); }
        });
    }

    protected override void CalculateReward(ABMAgent agent) {
        if(Distance(agent) < boundsRadius) agent.AddReward(0.01f);
    }

    private float Distance(ABMAgent a) {
        return Vector3.Distance(a.transform.localPosition, Vector3.zero);
    }
}