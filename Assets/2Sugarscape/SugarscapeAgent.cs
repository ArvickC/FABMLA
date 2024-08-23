using System;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SugarscapeAgent : ABMAgent
{
    public override int vectorObservationSpaceSize => 8;
    public override int vectorStackSize => 1;
    public override string behaviorName => "SugarscapeAgent";

    public int sugar;
    [HideInInspector] public float id;
    private int metabolism;
    private const int GridBoundary = 8;

    public override void Initialize()
    {
        base.Initialize();
        id = UnityEngine.Random.Range(0f, 100f);
        ResetAgent();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var sugarscapeController = controller.GetComponent<SugarscapeController>();
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(sugar);
        sensor.AddObservation(GetObservation(Vector3.left, sugarscapeController));
        sensor.AddObservation(GetObservation(Vector3.right, sugarscapeController));
        sensor.AddObservation(GetObservation(Vector3.down, sugarscapeController));
        sensor.AddObservation(GetObservation(Vector3.up, sugarscapeController));
    }

    private int GetObservation(Vector3 direction, SugarscapeController controller)
    {
        Vector3 newPosition = transform.localPosition + direction;
        if (IsWithinBounds(newPosition))
        {
            return controller.grid[newPosition].value;
        }
        return 0;
    }

    private bool IsWithinBounds(Vector3 position)
    {
        return Math.Abs(position.x) <= GridBoundary && Math.Abs(position.y) <= GridBoundary;
    }

    public override void Step()
    {
        base.Step();
        Move(actions.DiscreteActions[0]);
        ConsumeSugar();
    }

    private void Move(int action)
    {
        Vector3 movement = action switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,
            _ => Vector3.zero
        };
        this.transform.localPosition += movement;
    }

    private void ConsumeSugar()
    {
        sugar -= metabolism;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Agent")) return;

        var otherAgent = other.gameObject.GetComponent<SugarscapeAgent>();
        if (sugar < otherAgent.id || id < otherAgent.id)
        {
            controller.ClearAgent(this);
        }
        controller.CheckToEndEpisode();
    }

    private void ResetAgent()
    {
        ResetPosition();
        ResetVelocity();
        ResetMetabolism();
    }

    private void ResetPosition()
    {
        var sugarscapeController = controller.GetComponent<SugarscapeController>();
        int radius = sugarscapeController.radius;

        this.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-radius, radius),
            UnityEngine.Random.Range(-radius, radius),
            0
        );
    }

    private void ResetVelocity()
    {
        var rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void ResetMetabolism()
    {
        sugar = UnityEngine.Random.Range(5, 7);
        metabolism = UnityEngine.Random.Range(1, 3);
    }
}
