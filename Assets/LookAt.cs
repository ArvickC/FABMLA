using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Vector3 target = new Vector3(0, 0, 0);

    void Update() {
        transform.LookAt(target);
    }
}
