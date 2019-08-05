using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    Transform target;
    Vector3 initialPosition;
    static float pendingShakeDuration = 0f;
    float intensity = .3f;
    bool isShaking = false;

    void Start() {
        target = GetComponent<Transform>();
        initialPosition = target.localPosition;
    }
    public static void Shake(float duration) {
        if (duration > 0) {
            pendingShakeDuration += duration;
        }
    }

    void Update() {
        if (pendingShakeDuration > 0 && !isShaking) {
           StartCoroutine(DoShake()); 
        }
    }
    IEnumerator DoShake() {
        isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < startTime + pendingShakeDuration) {
            var randomPoint = new Vector3(Random.Range(-1f, 1f) * intensity, Random.Range(-1f, 1f) * intensity, initialPosition.z);
            target.localPosition = randomPoint;
            yield return null;
        }

        pendingShakeDuration = 0;
        target.localPosition = initialPosition;
        isShaking = false;
    }
}
