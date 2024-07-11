using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SugarblockManager : MonoBehaviour {
    public int value = 3; // Max = 3

    private void OnTriggerEnter(Collider o) {
        if(!o.gameObject.CompareTag("Agent")) return;
        SugarscapeAgent a = o.gameObject.GetComponent<SugarscapeAgent>();
        a.sugar += value;
    }
}
