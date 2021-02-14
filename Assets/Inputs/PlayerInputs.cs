// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""GameplayMap"",
            ""id"": ""6ee0f7e1-3d56-4d93-960b-4d313394c5a3"",
            ""actions"": [
                {
                    ""name"": ""LeftStick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6a9617f2-955b-4f9d-8f90-0337bb4d7b5b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightStick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2fe4cf52-eea8-44cf-b1e1-900b16532ba0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPad"",
                    ""type"": ""PassThrough"",
                    ""id"": ""14942c83-ea30-4cf7-b6f6-ec3b7202572d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftTrigger"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8ba2558d-8e24-4092-a1a0-05bce705590a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SouthButton"",
                    ""type"": ""Button"",
                    ""id"": ""769e2d19-8598-4654-b30b-f1188b39cdb2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""WestButton"",
                    ""type"": ""Button"",
                    ""id"": ""7d62326e-b4a9-413a-9365-a321d03691a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""EastButton"",
                    ""type"": ""Button"",
                    ""id"": ""95b066ed-9b17-454a-a38c-6326e20c50b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a515c30f-82c9-4272-b623-d994d942d7ba"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bd21660-3479-4a8f-a4fb-7ea1664b53c9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0cc3902a-4537-4d3a-88ea-9be702b6ca35"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05430b9b-410d-4abf-a134-181ba1f43d91"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53f8199f-2cd9-4d5d-b3b0-7ece3f12ffc1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SouthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""affde342-73e0-4bf0-931f-615124eb5336"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa9fd8ce-942d-4a8a-ac32-e8dbc51ef3da"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EastButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameplayMap
        m_GameplayMap = asset.FindActionMap("GameplayMap", throwIfNotFound: true);
        m_GameplayMap_LeftStick = m_GameplayMap.FindAction("LeftStick", throwIfNotFound: true);
        m_GameplayMap_RightStick = m_GameplayMap.FindAction("RightStick", throwIfNotFound: true);
        m_GameplayMap_DPad = m_GameplayMap.FindAction("DPad", throwIfNotFound: true);
        m_GameplayMap_LeftTrigger = m_GameplayMap.FindAction("LeftTrigger", throwIfNotFound: true);
        m_GameplayMap_SouthButton = m_GameplayMap.FindAction("SouthButton", throwIfNotFound: true);
        m_GameplayMap_WestButton = m_GameplayMap.FindAction("WestButton", throwIfNotFound: true);
        m_GameplayMap_EastButton = m_GameplayMap.FindAction("EastButton", throwIfNotFound: true);
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

    // GameplayMap
    private readonly InputActionMap m_GameplayMap;
    private IGameplayMapActions m_GameplayMapActionsCallbackInterface;
    private readonly InputAction m_GameplayMap_LeftStick;
    private readonly InputAction m_GameplayMap_RightStick;
    private readonly InputAction m_GameplayMap_DPad;
    private readonly InputAction m_GameplayMap_LeftTrigger;
    private readonly InputAction m_GameplayMap_SouthButton;
    private readonly InputAction m_GameplayMap_WestButton;
    private readonly InputAction m_GameplayMap_EastButton;
    public struct GameplayMapActions
    {
        private @PlayerInputs m_Wrapper;
        public GameplayMapActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftStick => m_Wrapper.m_GameplayMap_LeftStick;
        public InputAction @RightStick => m_Wrapper.m_GameplayMap_RightStick;
        public InputAction @DPad => m_Wrapper.m_GameplayMap_DPad;
        public InputAction @LeftTrigger => m_Wrapper.m_GameplayMap_LeftTrigger;
        public InputAction @SouthButton => m_Wrapper.m_GameplayMap_SouthButton;
        public InputAction @WestButton => m_Wrapper.m_GameplayMap_WestButton;
        public InputAction @EastButton => m_Wrapper.m_GameplayMap_EastButton;
        public InputActionMap Get() { return m_Wrapper.m_GameplayMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayMapActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayMapActions instance)
        {
            if (m_Wrapper.m_GameplayMapActionsCallbackInterface != null)
            {
                @LeftStick.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftStick;
                @LeftStick.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftStick;
                @LeftStick.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftStick;
                @RightStick.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnRightStick;
                @RightStick.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnRightStick;
                @RightStick.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnRightStick;
                @DPad.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnDPad;
                @DPad.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnDPad;
                @DPad.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnDPad;
                @LeftTrigger.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnLeftTrigger;
                @SouthButton.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnSouthButton;
                @SouthButton.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnSouthButton;
                @SouthButton.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnSouthButton;
                @WestButton.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnWestButton;
                @WestButton.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnWestButton;
                @WestButton.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnWestButton;
                @EastButton.started -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnEastButton;
                @EastButton.performed -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnEastButton;
                @EastButton.canceled -= m_Wrapper.m_GameplayMapActionsCallbackInterface.OnEastButton;
            }
            m_Wrapper.m_GameplayMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
                @DPad.started += instance.OnDPad;
                @DPad.performed += instance.OnDPad;
                @DPad.canceled += instance.OnDPad;
                @LeftTrigger.started += instance.OnLeftTrigger;
                @LeftTrigger.performed += instance.OnLeftTrigger;
                @LeftTrigger.canceled += instance.OnLeftTrigger;
                @SouthButton.started += instance.OnSouthButton;
                @SouthButton.performed += instance.OnSouthButton;
                @SouthButton.canceled += instance.OnSouthButton;
                @WestButton.started += instance.OnWestButton;
                @WestButton.performed += instance.OnWestButton;
                @WestButton.canceled += instance.OnWestButton;
                @EastButton.started += instance.OnEastButton;
                @EastButton.performed += instance.OnEastButton;
                @EastButton.canceled += instance.OnEastButton;
            }
        }
    }
    public GameplayMapActions @GameplayMap => new GameplayMapActions(this);
    public interface IGameplayMapActions
    {
        void OnLeftStick(InputAction.CallbackContext context);
        void OnRightStick(InputAction.CallbackContext context);
        void OnDPad(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnWestButton(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
    }
}
