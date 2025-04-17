using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public float mouseSensitivity = 5;

    public float cameraRotatingSpeed = 80;
    public float cameraYSpeed = 5;
    private Transform _target;
    private Transform _camera;
    [SerializeField] private AnimationCurve _armLengthCurve;

    private void Awake()
    {
        _camera = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void InitCamera(Transform target)
    {
        _target = target;
        transform.position = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdateArmLength();
    }


    private void UpdateRotation()
    {
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Yaw += Input.GetAxis("CameraRateX") * cameraRotatingSpeed * Time.deltaTime;
        Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch += Input.GetAxis("CameraRateY") * cameraRotatingSpeed * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, -90, 90);

        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);
    }

    private void UpdatePosition()
    {
        Vector3 position = _target.position;
        float newY = Mathf.Lerp(transform.position.y, position.y, Time.deltaTime * cameraYSpeed);
        transform.position = new Vector3(position.x, newY, position.z);
    }

    private void UpdateArmLength()
    {
        _camera.localPosition = new Vector3(0, 0, _armLengthCurve.Evaluate(Pitch) * -1);
    }
}
