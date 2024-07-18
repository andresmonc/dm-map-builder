using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rbody;
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 30f;

    private Vector2 previousMovementVector;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent -= HandleMove;
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        bodyTransform.Rotate(0f, 0f, previousMovementVector.x * -turningRate * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        // Calculate current position
        Vector2 currentPos = rbody.position;

        // Calculate desired movement based on input
        Vector2 desiredMovement = Vector2.zero;

        if (previousMovementVector.x == 1)
        {
            // Move right
            desiredMovement = Vector2.right;
        }
        else if (previousMovementVector.x == -1)
        {
            // Move left
            desiredMovement = Vector2.left;
        }
        else if (previousMovementVector.y == 1)
        {
            // Move up
            desiredMovement = Vector2.up;
        }
        else if (previousMovementVector.y == -1)
        {
            // Move down
            desiredMovement = Vector2.down;
        }

        // Calculate new position by snapping to nearest tile grid position
        Vector2 newPos = new Vector2(
            Mathf.Round(currentPos.x / 1f) * 1f,
            Mathf.Round(currentPos.y / 1f) * 1f
        );

        // Apply desired movement
        newPos += desiredMovement * 1f;

        // Apply the snapped position to Rigidbody2D
        rbody.MovePosition(newPos);
    }

    private void HandleMove(Vector2 vector)
    {
        previousMovementVector = vector;
    }
}
