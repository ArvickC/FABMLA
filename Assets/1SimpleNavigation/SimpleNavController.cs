using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleNavController : ABMController
{
    public float boundsRadius = 5f;
    public GameObject target;
    protected override void EndCase()
    {
        bool isEnd = false;
        agents.ForEach(a => {
            if(isEnd) return;
            if(Distance(a, new Vector3(0, 0, 0)) > boundsRadius) { isEnd = true; EndAll(); }
            if(Distance(a, target.transform.localPosition) < 1.5f) { a.AddReward(1.0f); isEnd = true; EndAll(); }
        });
    }

    protected override void CalculateReward(ABMAgent agent)
    {
        agent.AddReward(1f / (Distance(agent, target.transform.localPosition)+0.1f));
    }

    private float Distance(ABMAgent a, Vector3 t) {
        return Vector3.Distance(a.transform.localPosition, t);
    }

    protected override void SetupEpisode()
    {
        target.transform.localPosition = new Vector3(RandomRadius(), RandomRadius(), RandomRadius());
    }

    private float RandomRadius() {
        return Random.Range(-boundsRadius+0.5f, boundsRadius-0.5f);
    }
}
