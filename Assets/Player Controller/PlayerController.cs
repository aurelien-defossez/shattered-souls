using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FractiRetinae
{
	public class PlayerController : MonoBehaviourSingleton<PlayerController>
	{
		[SerializeField, Min(0)] private float walkSpeed = 1;
		[SerializeField, Min(0)] private float xSensitivity = 1;
		[SerializeField, Min(0)] private float ySensitivity = 1;
		[SerializeField, Min(0)] private float interactDistance = 1;
		[SerializeField] private Transform cameraContainer;

		public PlayerControls Controls { get; private set; }
		public CharacterController CharacterController { get; private set; }
		public Rigidbody Rigidbody { get; private set; }

		public Vector3 HeadPosition => cameraContainer.transform.position;
		public Vector3 LookDirection => cameras.First().transform.rotation * Vector3.forward;
		public Ray LookRay => new Ray(HeadPosition, LookDirection);

		private Camera[] cameras;

		protected override void Awake()
		{
			CharacterController = GetComponent<CharacterController>();
			Rigidbody = GetComponent<Rigidbody>();
			cameras = GetComponentsInChildren<Camera>();
			Controls = new PlayerControls();

			Controls.Player.Interact.performed += OnInteract;
		}

		protected void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			Controls.Player.Enable();
		}

		protected void Update()
		{
			// Read input
			Vector2 lookDirection = Controls.Player.Look.ReadValue<Vector2>();
			Vector2 movementDirection = Controls.Player.Move.ReadValue<Vector2>();

			// Look Horizontal
			transform.Rotate(Vector3.up, lookDirection.x * xSensitivity * Time.deltaTime);

			// Look Vertical
			float verticalRotation = cameraContainer.eulerAngles.x - lookDirection.y * ySensitivity * Time.deltaTime;
			cameraContainer.eulerAngles = cameraContainer.eulerAngles.WithX(verticalRotation);

			// Movement
			CharacterController.Move(transform.localRotation * new Vector3(movementDirection.x, Physics.gravity.y, movementDirection.y)
				* walkSpeed
				* Time.deltaTime
			);
		}

		public void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			CharacterController.enabled = false;
			transform.position = position;
			transform.rotation = rotation;
			CharacterController.enabled = true;
		}

		public Camera GetCameraFromLayer(int layer) => cameras[Convert.ToInt32(LayerMask.LayerToName(layer).Last().ToString()) - 1];

		private void OnInteract(InputAction.CallbackContext obj)
		{
			Debug.Log($"Raycast from {HeadPosition.ToString(4)} toward {LookDirection}");
			if (Physics.Raycast(LookRay, out RaycastHit hit, interactDistance))
			{
				Debug.Log($"Raycast hit {hit.collider.gameObject.name}");
				Switch interactableSwitch = hit.collider.GetComponent<Switch>();

				if (interactableSwitch != null)
				{
					interactableSwitch.Activate();
				}
			}
		}
	}
}
