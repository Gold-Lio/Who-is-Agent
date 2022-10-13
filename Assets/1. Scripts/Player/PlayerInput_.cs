using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerInput_ : MonoBehaviourPun
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack, OnItemPickUp, OnItemUse;

    private PlayerInputAction inputAction;

    private Vector2 movementPos = Vector2.zero;
    private Vector2 pointerPos = Vector2.zero;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        OnMovementInput?.Invoke(movementPos);
        OnPointerInput?.Invoke(GetPointerInput());
    }

    //기본적인 마우스ㅡ
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPos;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnEnable()
    {
        inputAction.PlayerInput.Enable();
        inputAction.PlayerInput.Movement.performed += Move;
        inputAction.PlayerInput.Movement.canceled += Move;
        inputAction.PlayerInput.PointerPosition.performed += PointerMove;
        inputAction.PlayerInput.Attack.performed += PerformAttack;
        inputAction.PlayerInput.ItemPickUp.performed += ItemPickUp;
        inputAction.Item.Enable();
        inputAction.Item.Use.performed += ItemUse;
    }

    private void PointerMove(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        pointerPos = obj.ReadValue<Vector2>();
    }

    private void Move(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        movementPos = obj.ReadValue<Vector2>().normalized;
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        OnAttack?.Invoke();
    }

    private void ItemPickUp(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        OnItemPickUp?.Invoke();
    }

    private void ItemUse(InputAction.CallbackContext obj)
    {
        if(!photonView.IsMine)
        {
            return;
        }
        OnItemUse?.Invoke();
    }

    private void OnDisable()
    {
        inputAction.Item.Use.performed -= ItemUse;
        inputAction.Item.Disable();
        inputAction.PlayerInput.ItemPickUp.performed -= ItemPickUp;
        inputAction.PlayerInput.Attack.performed -= PerformAttack;
        inputAction.PlayerInput.PointerPosition.performed -= PointerMove;
        inputAction.PlayerInput.Movement.canceled -= Move;
        inputAction.PlayerInput.Movement.performed -= Move;
        inputAction.PlayerInput.Disable();
    }
}
