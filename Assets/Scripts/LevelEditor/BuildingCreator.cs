using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingCreator : Singleton<BuildingCreator>
{
    PlayerInput playerInput;

    Vector2 mousePos;

    protected override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

}
