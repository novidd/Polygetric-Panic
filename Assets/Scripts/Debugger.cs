using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {
    private void OnDestroy() {
        Debug.LogWarning("I just got destroyed!", this);
    }
}
