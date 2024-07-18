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
    [SerializeField] private float movementSpeed = 5f;

    private bool moving = false;
    private Vector2 targetPosition;
    private Vector2 lastPosition;

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
        // rotate body here??
        // bodyTransform.Rotate(0f, 0f, previousMovementVector.x * -turningRate * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector2 currentPos = rbody.position;
        if (moving && currentPos == lastPosition)
        {
            moving = false;
            targetPosition = currentPos;
        }
        if (!moving && previousMovementVector != Vector2.zero)
        {
            targetPosition = previousMovementVector + currentPos;
            // Round to nearest half to remain centered in tiles
            targetPosition.x = (float)Math.Round(targetPosition.x * 2) / 2;
            targetPosition.y = (float)Math.Round(targetPosition.y * 2) / 2;

            Debug.Log(targetPosition);
            moving = true;
        }
        if (moving)
        {
            Vector2 newPos = Vector2.Lerp(currentPos, targetPosition, movementSpeed * Time.fixedDeltaTime);
            if (Vector2.Distance(newPos, targetPosition) <= 0.05f)
            {
                newPos = targetPosition;
                moving = false;
            }
            rbody.MovePosition(newPos);
        }
        // Update lastPosition
        lastPosition = currentPos;

    }

    private void HandleMove(Vector2 vector)
    {
        previousMovementVector = vector;
    }
}
