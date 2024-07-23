using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraZoom : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float zoomSpeed = 2f; // Adjust this value for zoom speed
    [SerializeField] private float minZoom = 2f; // Minimum zoom limit
    [SerializeField] private float maxZoom = 10f; // Maximum zoom limit

    private void OnEnable()
    {
        inputReader.ScrollEvent += HandleScroll;
    }

    private void OnDisable()
    {
        inputReader.ScrollEvent -= HandleScroll;
    }

    private void HandleScroll(float scrollValue)
    {
        float newOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize - scrollValue * zoomSpeed;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(newOrthographicSize, minZoom, maxZoom);
    }

}
