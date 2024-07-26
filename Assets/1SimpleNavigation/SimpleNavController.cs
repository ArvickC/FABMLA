using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleNavController : ABMController {
    public float boundsRadius = 5f;
    public GameObject target;
    int step = 0;
    bool reachedTarget;

    protected override void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            step = a.StepCount;
            if(isEnd) return;

            if(a.MaxStep > 0 && a.StepCount >= a.MaxStep) { isEnd = true; reachedTarget = false; EndAll(); } // Max stepcount reached
            if(Distance(a, new Vector3(0, 0, 0)) > boundsRadius) { isEnd = true; reachedTarget = false; EndAll(); } // Out of bounds
            if(Distance(a, target.transform.localPosition) < 1.5f) { a.AddReward(1.0f); reachedTarget = true; isEnd = true; EndAll(); } // Reached target
        });
    }

    protected override void CalculateReward(ABMAgent agent) {
        agent.AddReward(1f / (Distance(agent, target.transform.localPosition)+0.1f));
    }

    // Distance of Agent (a) to target (t)
    private float Distance(ABMAgent a, Vector3 t) {
        return Vector3.Distance(a.transform.localPosition, t);
    }

    protected override void SetupEpisode() {
        base.SetupEpisode();
        target.transform.localPosition = new Vector3(RandomRadius(1.5f), RandomRadius(1.5f), RandomRadius(1.5f)); // Spawn Randomly
    }

    public float RandomRadius(float sub) {
        return UnityEngine.Random.Range(-boundsRadius+sub, boundsRadius-sub);
    }

    protected override String Log() {
        return $"{step},{reachedTarget}";
    }
}
