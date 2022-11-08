using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerUI_ : MonoBehaviourPun
{
    public UnityEvent OnInventory, OnInteraction, OnItemCreate, OnItemDestroy;

    private PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.PlayerUI.Enable();
        inputAction.PlayerUI.InventoryOnOff.performed += InventoryOnOff;
        inputAction.PlayerUI.Interaction.performed += Interaction;
        inputAction.Item.Enable();
        inputAction.Item.Create.performed += ItemCreate;
        inputAction.Item.Destroy.performed += ItemDestroy;
    }

    private void ItemDestroy(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine) return;

        OnItemDestroy?.Invoke();        
    }

    private void ItemCreate(InputAction.CallbackContext obj)
    {
        if (!photonView.IsMine) return;

        OnItemCreate?.Invoke();
    }

    private void Interaction(InputAction.CallbackContext _)
    {
        if (!photonView.IsMine) return;

        OnInteraction?.Invoke();
    }

    private void InventoryOnOff(InputAction.CallbackContext _)
    {
        if (!photonView.IsMine) return;

        OnInventory?.Invoke();
    }

    private void OnDisable()
    {
        //inputAction.Item.Destroy.performed -= ItemDestroy;
        //inputAction.Item.Create.performed -= ItemCreate;
        //inputAction.Item.Disable();
        inputAction.PlayerUI.Interaction.performed -= Interaction;
        inputAction.PlayerUI.InventoryOnOff.performed -= InventoryOnOff;
        inputAction.PlayerUI.Disable();
    }
}
