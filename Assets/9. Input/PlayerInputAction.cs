// GENERATED AUTOMATICALLY FROM 'Assets/9. Input/PlayerInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""PlayerInput"",
            ""id"": ""bc5d3c9b-9bd0-4e72-8c7e-c88d5a5ea777"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""7c06ee74-7111-4875-9f7c-d47338a8226e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""e3a264ed-a5a8-4359-85fb-f30fc4fc87bd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""09564136-90f3-47a7-a37a-1fb4e7b3d751"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemPickUp"",
                    ""type"": ""Button"",
                    ""id"": ""6316899a-3b80-455e-8df3-fd332da0d56b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8be1b882-642a-4aea-bca2-76d1f72f1359"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99ba20db-1bbc-4a01-91f9-6e442b38acdd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""09420fbb-6e71-469f-8e12-5f584b0e192e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f6f76b99-870a-47a4-a96d-70556e62b313"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""103f4acc-040d-4dce-a835-0fa234d55a9b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2a221895-8e04-4147-b3a6-23a9c7388119"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""430caf33-2c77-44d2-abeb-dd0eb625e3d5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4477ccfc-11e8-4cde-91cf-c283dc59a53c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemPickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerUI"",
            ""id"": ""108a1936-f601-42c4-ba65-432e1438e509"",
            ""actions"": [
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""70816ad6-6244-44c8-8eba-b941137a5c08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InventoryOnOff"",
                    ""type"": ""Button"",
                    ""id"": ""c3b3e214-d8f1-4fcb-81a4-440300a545dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""bb682aa3-7e7d-4bea-b1a7-f4f5bf5d24cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a844ec85-7873-4ffe-a9b7-81b285fb0454"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c921bb8-7f08-4f6d-9292-b344728e1eae"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryOnOff"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aed8a569-645c-4e8d-99dd-023bb4fbb2d8"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Item"",
            ""id"": ""874ae9e9-5877-4bad-87e8-48615068a7c1"",
            ""actions"": [
                {
                    ""name"": ""Create"",
                    ""type"": ""Button"",
                    ""id"": ""fcbf48f4-5179-4a76-98a9-82d1a866e075"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Destroy"",
                    ""type"": ""Button"",
                    ""id"": ""a34717da-d2d3-4fa0-8a8a-dade70bc3f43"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""6ab37234-dd17-4a38-9a3d-f8b331de0eff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3127e31-3de8-448f-bf99-68317f88756e"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Create"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f347d94-b1df-4e60-a127-4a7d4476f440"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Destroy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ddc4709-4b93-4929-baca-f6447a8d318f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerInput
        m_PlayerInput = asset.FindActionMap("PlayerInput", throwIfNotFound: true);
        m_PlayerInput_Movement = m_PlayerInput.FindAction("Movement", throwIfNotFound: true);
        m_PlayerInput_PointerPosition = m_PlayerInput.FindAction("PointerPosition", throwIfNotFound: true);
        m_PlayerInput_Attack = m_PlayerInput.FindAction("Attack", throwIfNotFound: true);
        m_PlayerInput_ItemPickUp = m_PlayerInput.FindAction("ItemPickUp", throwIfNotFound: true);
        // PlayerUI
        m_PlayerUI = asset.FindActionMap("PlayerUI", throwIfNotFound: true);
        m_PlayerUI_Drop = m_PlayerUI.FindAction("Drop", throwIfNotFound: true);
        m_PlayerUI_InventoryOnOff = m_PlayerUI.FindAction("InventoryOnOff", throwIfNotFound: true);
        m_PlayerUI_Interaction = m_PlayerUI.FindAction("Interaction", throwIfNotFound: true);
        // Item
        m_Item = asset.FindActionMap("Item", throwIfNotFound: true);
        m_Item_Create = m_Item.FindAction("Create", throwIfNotFound: true);
        m_Item_Destroy = m_Item.FindAction("Destroy", throwIfNotFound: true);
        m_Item_Use = m_Item.FindAction("Use", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerInput
    private readonly InputActionMap m_PlayerInput;
    private IPlayerInputActions m_PlayerInputActionsCallbackInterface;
    private readonly InputAction m_PlayerInput_Movement;
    private readonly InputAction m_PlayerInput_PointerPosition;
    private readonly InputAction m_PlayerInput_Attack;
    private readonly InputAction m_PlayerInput_ItemPickUp;
    public struct PlayerInputActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerInputActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerInput_Movement;
        public InputAction @PointerPosition => m_Wrapper.m_PlayerInput_PointerPosition;
        public InputAction @Attack => m_Wrapper.m_PlayerInput_Attack;
        public InputAction @ItemPickUp => m_Wrapper.m_PlayerInput_ItemPickUp;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerInputActions instance)
        {
            if (m_Wrapper.m_PlayerInputActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnMovement;
                @PointerPosition.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnPointerPosition;
                @Attack.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnAttack;
                @ItemPickUp.started -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnItemPickUp;
                @ItemPickUp.performed -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnItemPickUp;
                @ItemPickUp.canceled -= m_Wrapper.m_PlayerInputActionsCallbackInterface.OnItemPickUp;
            }
            m_Wrapper.m_PlayerInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @ItemPickUp.started += instance.OnItemPickUp;
                @ItemPickUp.performed += instance.OnItemPickUp;
                @ItemPickUp.canceled += instance.OnItemPickUp;
            }
        }
    }
    public PlayerInputActions @PlayerInput => new PlayerInputActions(this);

    // PlayerUI
    private readonly InputActionMap m_PlayerUI;
    private IPlayerUIActions m_PlayerUIActionsCallbackInterface;
    private readonly InputAction m_PlayerUI_Drop;
    private readonly InputAction m_PlayerUI_InventoryOnOff;
    private readonly InputAction m_PlayerUI_Interaction;
    public struct PlayerUIActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerUIActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Drop => m_Wrapper.m_PlayerUI_Drop;
        public InputAction @InventoryOnOff => m_Wrapper.m_PlayerUI_InventoryOnOff;
        public InputAction @Interaction => m_Wrapper.m_PlayerUI_Interaction;
        public InputActionMap Get() { return m_Wrapper.m_PlayerUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerUIActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerUIActions instance)
        {
            if (m_Wrapper.m_PlayerUIActionsCallbackInterface != null)
            {
                @Drop.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnDrop;
                @InventoryOnOff.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInventoryOnOff;
                @InventoryOnOff.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInventoryOnOff;
                @InventoryOnOff.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInventoryOnOff;
                @Interaction.started -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_PlayerUIActionsCallbackInterface.OnInteraction;
            }
            m_Wrapper.m_PlayerUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @InventoryOnOff.started += instance.OnInventoryOnOff;
                @InventoryOnOff.performed += instance.OnInventoryOnOff;
                @InventoryOnOff.canceled += instance.OnInventoryOnOff;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
            }
        }
    }
    public PlayerUIActions @PlayerUI => new PlayerUIActions(this);

    // Item
    private readonly InputActionMap m_Item;
    private IItemActions m_ItemActionsCallbackInterface;
    private readonly InputAction m_Item_Create;
    private readonly InputAction m_Item_Destroy;
    private readonly InputAction m_Item_Use;
    public struct ItemActions
    {
        private @PlayerInputAction m_Wrapper;
        public ItemActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Create => m_Wrapper.m_Item_Create;
        public InputAction @Destroy => m_Wrapper.m_Item_Destroy;
        public InputAction @Use => m_Wrapper.m_Item_Use;
        public InputActionMap Get() { return m_Wrapper.m_Item; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ItemActions set) { return set.Get(); }
        public void SetCallbacks(IItemActions instance)
        {
            if (m_Wrapper.m_ItemActionsCallbackInterface != null)
            {
                @Create.started -= m_Wrapper.m_ItemActionsCallbackInterface.OnCreate;
                @Create.performed -= m_Wrapper.m_ItemActionsCallbackInterface.OnCreate;
                @Create.canceled -= m_Wrapper.m_ItemActionsCallbackInterface.OnCreate;
                @Destroy.started -= m_Wrapper.m_ItemActionsCallbackInterface.OnDestroy;
                @Destroy.performed -= m_Wrapper.m_ItemActionsCallbackInterface.OnDestroy;
                @Destroy.canceled -= m_Wrapper.m_ItemActionsCallbackInterface.OnDestroy;
                @Use.started -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
            }
            m_Wrapper.m_ItemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Create.started += instance.OnCreate;
                @Create.performed += instance.OnCreate;
                @Create.canceled += instance.OnCreate;
                @Destroy.started += instance.OnDestroy;
                @Destroy.performed += instance.OnDestroy;
                @Destroy.canceled += instance.OnDestroy;
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
            }
        }
    }
    public ItemActions @Item => new ItemActions(this);
    public interface IPlayerInputActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnItemPickUp(InputAction.CallbackContext context);
    }
    public interface IPlayerUIActions
    {
        void OnDrop(InputAction.CallbackContext context);
        void OnInventoryOnOff(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
    }
    public interface IItemActions
    {
        void OnCreate(InputAction.CallbackContext context);
        void OnDestroy(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
    }
}
