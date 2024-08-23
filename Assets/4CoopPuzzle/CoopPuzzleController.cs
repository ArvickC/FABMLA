using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoopPuzzleController : ABMController {
    [Header("Env")]
    public GameObject target;
    public GameObject plate;
    public Vector3 spawnMin;
    public Vector3 spawnMax;
    public List<int> agentsOnPlate;
    public List<int> agentsOnTarget;

    [Header("Agents")]
    public Vector3 spawnMinAgent;
    public Vector3 spawnMaxAgent;

    private SimpleMultiAgentGroup agentGroup = new SimpleMultiAgentGroup();
    int step;
    bool success = false;

    protected override void Start() {
        base.Start();
        StartCoroutine(StartSimulation(0.25f));
    }

    protected override void EndCase() {
        if(agentsOnPlate.Count == 1 && agentsOnTarget.Count == 1) {
            AddGroupReward(10f);
            EndEpisode();
            return;
        }
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            step = a.StepCount;
            if(isEnd) return;
            if(a.MaxStep > 0 && a.StepCount >= a.MaxStep) { isEnd = true; EndEpisode(true); } // Max stepcount reached
        });       
    }

    protected override void CalculateReward(ABMAgent agent) {
        CoopPuzzleAgent a = (CoopPuzzleAgent)agent;
        if(a.onOnePlate) a.AddReward(0.005f);

        float z1 = agents[0].transform.localPosition.z;
        float z2 = agents[1].transform.localPosition.z;

        if(z1 < 0 && z2 < 0) a.AddReward(-0.001f);
        if(z1 > 0 && z2 > 0) a.AddReward(-0.001f);
    }

    protected override void SetupEpisode()
    {
        base.SetupEpisode();
        SpawnPlate();
        SpawnTarget();
        SpawnAgents();
    }
    
    private void SpawnPlate()
    {
        plate.transform.localPosition = GetRandomPosition(spawnMin, spawnMax);
    }
    
    private void SpawnTarget()
    {
        Vector3 targetSpawnMin = new Vector3(spawnMin.x, spawnMin.y, -spawnMax.z);
        Vector3 targetSpawnMax = new Vector3(spawnMax.x, spawnMax.y, -spawnMin.z);
        target.transform.localPosition = GetRandomPosition(targetSpawnMin, targetSpawnMax);
    }
    
    private void SpawnAgents()
    {
        agents.ToList().ForEach(SpawnAgent);
    }
    
    private void SpawnAgent(ABMAgent agent)
    {
        agent.transform.localPosition = GetRandomPosition(spawnMinAgent, spawnMaxAgent);
        agent.transform.localRotation = GetRandomRotation();
    }
    
    private Vector3 GetRandomPosition(Vector3 min, Vector3 max)
    {
        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }
    
    private Quaternion GetRandomRotation()
    {
        return Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
    }

    public void AddGroupReward(float r) {
        agentGroup.AddGroupReward(r);
    }

    public void EndEpisode() {
        success = true;
        agentGroup.EndGroupEpisode();
        SetupEpisode();
    }

    public void EndEpisode(bool interupt) {
        if(interupt) {
            success = false;
            agentGroup.GroupEpisodeInterrupted();
        }
        SetupEpisode();
    }

    IEnumerator StartSimulation(float s) {
        yield return new WaitForSeconds(s);
        agents.ToList().ForEach(a => agentGroup.RegisterAgent(a));
        EndEpisode(true);
    }

    protected override System.String Log() {
        return $"{step},{success}";
    }
}
