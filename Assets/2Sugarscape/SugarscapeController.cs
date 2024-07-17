using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

public class SugarscapeController : ABMController {
    public GameObject gridObject;
    public List<Material> sugarMaterials = new List<Material>();
    public Dictionary<Vector3, SugarblockManager> grid = new Dictionary<Vector3, SugarblockManager>();
    public int radius = 8;

    protected override void Start() {
        SetupEpisode();
    }

    protected override void EndCase() {
        bool isEnd = false;
        agents.ToList().ForEach(a => {
            if(isEnd) return;

            if(a.MaxStep > 0 && a.StepCount >= a.MaxStep) {isEnd = true; EndAll();} // Step count reached
            if(a.GetComponent<SugarscapeAgent>().sugar <= 0) { a.AddReward(-1.0f); ClearAgent(a);} // Out of sugar
            if(Math.Abs(a.transform.localPosition.x) > 8 || Math.Abs(a.transform.localPosition.y) > 8) 
                { a.AddReward(-1.0f); ClearAgent(a); } // Out of bounds
        });

        if(agents.Count <= 1) { SetupEpisode(); } // Only 1 agent left
    }

    protected override void CalculateReward(ABMAgent agent) {
        agent.AddReward(agent.GetComponent<SugarscapeAgent>().sugar / 100f);
    }

    protected override void SetupEpisode() {
        // Clear scene
        ClearAgents();
        grid.Keys.ToList().ForEach(k => {
            Destroy(grid[k].gameObject);
        });
        grid.Clear();

        // Setup new grid
        for(int x=-radius;x<=radius;x++) {
            for(int y=-radius;y<=radius;y++) {
                SetGrid(x, y);
            }
        }
        
        for(int i=0;i<agentAmount;i++) {
            GameObject a = Instantiate(simulationAgent); // Make agents
        }
    }

    private void SetGrid(int x, int y) {
        GameObject g = Instantiate(gridObject); // Make sugar cube
        g.transform.localPosition = new Vector3(x, y, 0f);
        
        int r = UnityEngine.Random.Range(1, 10);
        if(r <= 5) r = 1; // 50% white
        else if (r <= 8) r = 2; // 30% yellow
        else r = 3; // 20% red

        // Set cube value
        g.GetComponent<SugarblockManager>().value = r;
        List<Material> m = new List<Material>();
        m.Add(sugarMaterials[r-1]);
        g.GetComponent<MeshRenderer>().SetMaterials(m);

        grid.Add(g.transform.localPosition, g.GetComponent<SugarblockManager>()); // Register cube
    }
}
