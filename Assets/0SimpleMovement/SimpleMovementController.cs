using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Linq;

public class SimpleMovementController : ABMController {
    public float boundsRadius = 5f;

    protected override void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            if(isEnd) { return; }

            if(Distance(a)>boundsRadius || a.StepCount >= a.MaxStep) { isEnd = true; EndAll(); } // Out of bounds
        });
    }

    protected override void CalculateReward(ABMAgent agent) {
        if(Distance(agent) < boundsRadius) agent.AddReward(0.01f);
    }

    // Calculate distance from center
    private float Distance(ABMAgent a) {
        return Vector3.Distance(a.transform.localPosition, Vector3.zero);
    }
}