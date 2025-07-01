using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DM
{
    public class User : MonoBehaviour
    {
        private RenderSettings renderSettings;

        private Camera camera_;

        private float zoomSpeed;

        private UserInputActions userInputActions;
        private InputAction zoomAction;

        void Awake()
        {
            renderSettings = Resources.Load<RenderSettings>("Settings/RenderSettings");

            camera_ = GameObject.Find("User").GetComponentInChildren<Camera>();
            camera_.transform.position = new Vector3(0, 0, -10);
            camera_.orthographicSize = renderSettings.defaultZoom;

            zoomSpeed = renderSettings.zoomSpeed;

            userInputActions = new UserInputActions();

            zoomAction = userInputActions.User.Zoom;

        }

        void OnEnable()
        {
            zoomAction.Enable();
        }

        void Update()
        {
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            float zoomValue = zoomAction.ReadValue<float>();
            float zoomDisplacement = zoomValue * zoomSpeed;

            camera_.orthographicSize = Mathf.Lerp(
                camera_.orthographicSize,
                camera_.orthographicSize + zoomDisplacement,
                Time.deltaTime
            );

            camera_.orthographicSize = Mathf.Clamp(
                camera_.orthographicSize,
                renderSettings.minZoom,
                renderSettings.maxZoom
            );
        }

        void OnDisable()
        {
            zoomAction.Disable();
        }
    }
}
