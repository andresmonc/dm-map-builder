using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TilemapCameraManager : Singleton<TilemapCameraManager>
{
    [SerializeField] private LevelEditorInputReader inputReader;
    [SerializeField] private float zoomSpeed = 2f; // Adjust this value for zoom speed
    [SerializeField] private float minZoom = 2f; // Minimum zoom limit
    [SerializeField] private float maxZoom = 10f; // Maximum zoom limit
    [SerializeField] private float panSpeed = 0.1f; // Panning speed default


    private Camera _camera;
    private bool panCamera;
    private Vector2 lastMousePosition;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (panCamera)
        {
            if (lastMousePosition == inputReader.MousePosition) { return; }
            // Invert direction
            Vector2 positionChange = lastMousePosition - inputReader.MousePosition;
            // Convert to Vector3 (assuming panning on the x and y axes)
            Vector3 positionChange3D = new Vector3(positionChange.x, positionChange.y, 0) * panSpeed;
            // Apply the position change to the camera
            _camera.transform.position += positionChange3D;
        }
        lastMousePosition = inputReader.MousePosition;

    }

    private void OnEnable()
    {
        inputReader.ScrollEvent += HandleScroll;
        inputReader.MiddleClickEvent += (panCamera) => this.panCamera = panCamera;
    }

    private void OnDisable()
    {
        inputReader.ScrollEvent -= HandleScroll;
        inputReader.MiddleClickEvent -= (panCamera) => this.panCamera = panCamera;
    }

    private void HandleScroll(float scrollValue)
    {
        _camera.orthographicSize -= scrollValue * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
    }

}
