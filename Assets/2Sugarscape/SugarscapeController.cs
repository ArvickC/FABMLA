using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SugarscapeController : ABMController
{
    [SerializeField] private GameObject gridObject;
    [SerializeField] private List<Material> sugarMaterials = new List<Material>();
    
    private Dictionary<Vector3, SugarblockManager> grid = new Dictionary<Vector3, SugarblockManager>();
    private const int Radius = 8;

    protected override void Start()
    {
        SetupEpisode();
    }

    protected override void EndCase()
    {
        if (CheckEndConditions())
        {
            SetupEpisode();
        }
    }

    protected override void CalculateReward(ABMAgent agent)
    {
        var sugarscapeAgent = agent.GetComponent<SugarscapeAgent>();
        agent.AddReward(sugarscapeAgent.sugar / 100f);
    }

    protected override void SetupEpisode()
    {
        ClearScene();
        SetupGrid();
        SpawnAgents();
    }

    private bool CheckEndConditions()
    {
        bool isEnd = false;
        agents.ToList().ForEach(a =>
        {
            if (isEnd) return;
            
            var sugarscapeAgent = a.GetComponent<SugarscapeAgent>();
            
            if (a.MaxStep > 0 && a.StepCount >= a.MaxStep)
            {
                isEnd = true;
                EndAll();
            }
            else if (sugarscapeAgent.sugar <= 0)
            {
                a.AddReward(-1.0f);
                ClearAgent(a);
            }
            else if (Math.Abs(a.transform.localPosition.x) > Radius || Math.Abs(a.transform.localPosition.y) > Radius)
            {
                a.AddReward(-1.0f);
                ClearAgent(a);
            }
        });

        return isEnd || agents.Count <= 1;
    }

    private void ClearScene()
    {
        ClearAgents();
        foreach (var key in grid.Keys.ToList())
        {
            Destroy(grid[key].gameObject);
        }
        grid.Clear();
    }

    private void SetupGrid()
    {
        for (int x = -Radius; x <= Radius; x++)
        {
            for (int y = -Radius; y <= Radius; y++)
            {
                SetGrid(x, y);
            }
        }
    }

    private void SpawnAgents()
    {
        for (int i = 0; i < agentAmount; i++)
        {
            Instantiate(simulationAgent);
        }
    }

    private void SetGrid(int x, int y)
    {
        GameObject g = Instantiate(gridObject);
        g.transform.localPosition = new Vector3(x, y, 0f);

        int value = DetermineSugarValue();
        SetSugarBlock(g, value);

        grid.Add(g.transform.localPosition, g.GetComponent<SugarblockManager>());
    }

    private int DetermineSugarValue()
    {
        int r = UnityEngine.Random.Range(1, 10);
        if (r <= 5) return 1; // 50% white
        if (r <= 8) return 2; // 30% yellow
        return 3; // 20% red
    }

    private void SetSugarBlock(GameObject sugarBlock, int value)
    {
        var sugarblockManager = sugarBlock.GetComponent<SugarblockManager>();
        sugarblockManager.value = value;

        var meshRenderer = sugarBlock.GetComponent<MeshRenderer>();
        meshRenderer.SetMaterials(new List<Material> { sugarMaterials[value - 1] });
    }
}
