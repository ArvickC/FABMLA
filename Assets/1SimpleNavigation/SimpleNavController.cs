using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleNavController : ABMController
{
    public float boundsRadius = 5f;
    public GameObject target;
    private int step = 0;
    private bool reachedTarget;
    private const float TargetReachDistance = 1.5f;

    protected override void EndCase()
    {
        bool isEnd = CheckEndConditions();
        if (isEnd) EndAll();
    }

    private bool CheckEndConditions()
    {
        return agents.Any(agent =>
        {
            step = agent.StepCount;

            if (agent.MaxStep > 0 && agent.StepCount >= agent.MaxStep)
            {
                reachedTarget = false;
                return true;
            }

            if (IsOutOfBounds(agent))
            {
                reachedTarget = false;
                return true;
            }

            if (HasReachedTarget(agent))
            {
                agent.AddReward(1.0f);
                reachedTarget = true;
                return true;
            }

            return false;
        });
    }

    private bool IsOutOfBounds(ABMAgent agent)
    {
        return Distance(agent, Vector3.zero) > boundsRadius;
    }

    private bool HasReachedTarget(ABMAgent agent)
    {
        return Distance(agent, target.transform.localPosition) < TargetReachDistance;
    }

    protected override void CalculateReward(ABMAgent agent)
    {
        agent.AddReward(1f / (Distance(agent, target.transform.localPosition) + 0.1f));
    }

    private float Distance(ABMAgent agent, Vector3 targetPosition)
    {
        return Vector3.Distance(agent.transform.localPosition, targetPosition);
    }

    protected override void SetupEpisode()
    {
        base.SetupEpisode();
        SpawnTarget();
    }

    private void SpawnTarget()
    {
        target.transform.localPosition = new Vector3(
            RandomRadius(TargetReachDistance),
            RandomRadius(TargetReachDistance),
            RandomRadius(TargetReachDistance)
        );
    }

    private float RandomRadius(float buffer)
    {
        return UnityEngine.Random.Range(-boundsRadius + buffer, boundsRadius - buffer);
    }

    protected override string Log()
    {
        return $"{step},{reachedTarget}";
    }
}
