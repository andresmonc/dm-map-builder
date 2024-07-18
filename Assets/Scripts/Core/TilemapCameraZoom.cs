using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class TilemapCameraZoom : Singleton<TilemapCameraManager>
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float zoomSpeed = 2f; // Adjust this value for zoom speed
    [SerializeField] private float minZoom = 2f; // Minimum zoom limit
    [SerializeField] private float maxZoom = 10f; // Maximum zoom limit


    private Camera _camera;


    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

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
        _camera.orthographicSize -= scrollValue * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
    }

}
