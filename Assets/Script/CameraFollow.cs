using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 distance = new Vector3(0, 8, -18);
    public Camera camera;
    public Vector3 offset = new Vector3(0, 5f, 0);
    public float speed = 3f;
    Vector3 pos;
    Vector3 forward;
    Vector3 targetPos;
    private void Start()
    {
        camera = Camera.main;
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        Vector3 initpos = pos - 30 * forward + Vector3.up * 10;
        camera.transform.position = initpos;
    }
    public void LateUpdata()
    {
        pos = transform.position;
        forward = transform.forward;
        targetPos = pos + forward * distance.z;
        targetPos.y += distance.y;
        Vector3 cameraPos = camera.transform.position;
        cameraPos = Vector3.MoveTowards(cameraPos, targetPos, Time.deltaTime * speed);
        camera.transform.position = cameraPos;
        camera.transform.LookAt(pos + offset);
    }
}
