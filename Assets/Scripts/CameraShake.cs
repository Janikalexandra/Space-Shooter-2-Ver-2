using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Transform cameraTransform = default;

    private Vector3 _originalPosOfCamera = default;

    public float shakeFrequency = default;

    //Rotate camera in the x, y axes 

    void Start()
    {
        _originalPosOfCamera = cameraTransform.position;
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            CameraShakes();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopShake();
        } */      
    }

    public IEnumerator CameraIsShaking()
    {
        cameraTransform.position = _originalPosOfCamera + Random.insideUnitSphere * shakeFrequency;
        yield return new WaitForSeconds(0.1f);
        cameraTransform.position = _originalPosOfCamera;
    }

    public void CameraShakes()
    {
        cameraTransform.position = _originalPosOfCamera + Random.insideUnitSphere * shakeFrequency;
    }

    private void StopShake()
    {
        cameraTransform.position = _originalPosOfCamera;
    }
}
