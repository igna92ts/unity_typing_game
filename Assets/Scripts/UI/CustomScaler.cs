using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomScaler : MonoBehaviour {
    public CanvasScaler canvasScaler;
    void Start() {
        
        canvasScaler = GetComponent<CanvasScaler>();
        if (Camera.main.aspect > 1) {
            canvasScaler.referenceResolution = new Vector2(1980, 1080);
        } else {
            canvasScaler.referenceResolution = new Vector2(800, 600);
        }
    }
}
