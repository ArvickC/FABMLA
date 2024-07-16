using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {
    public GameObject ball;
    public Teams team;

    [HideInInspector] public enum Teams {Blue, Purple};
}
