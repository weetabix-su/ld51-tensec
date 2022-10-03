using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followObject;
    public Rect cameraLimits = new Rect(0, 0, 10, 10);
    [Range(0f, 1f)] public float followSpeed = 0.75f;

    private void Update()
    {
        transform.position = followObject != null ? Vector3.Lerp(transform.position, new Vector3(
            Mathf.Clamp(followObject.position.x, cameraLimits.x - (cameraLimits.width * 0.5f), cameraLimits.x + (cameraLimits.width * 0.5f)),
            Mathf.Clamp(followObject.position.y, cameraLimits.y - (cameraLimits.height * 0.5f), cameraLimits.y + (cameraLimits.height * 0.5f)),
            transform.position.z), followSpeed) : Vector3.Lerp(transform.position, new Vector3(0f,0f,transform.position.z), followSpeed);
    }

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
