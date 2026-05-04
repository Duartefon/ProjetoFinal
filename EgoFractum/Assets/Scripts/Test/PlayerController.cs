using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] float walkSpeed = 4f;
        [SerializeField] float sensitivity = 1f;
        [SerializeField] float playerHeight = 1.9f;
        [SerializeField] float gravity = 10;    

        [Header("View Bobbing")]
        [SerializeField] private float bobFrequency = 3f;
        [SerializeField] private float bobHorizontalAmplitude = 0.07f;
        [SerializeField] private float bobVerticalAmplitude = 0.15f;
        [SerializeField] private float bobSmoothing = 8f;

        private float bobTimer;
        private Vector3 cameraDefaultLocalPos;
        private InputManager playerInput;
        private CharacterController _characterController;
        private CapsuleCollider _collider;
        private Camera _camera;
        private float xRotation, yRotation;
        private float currentSpeed;


        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _collider = GetComponent<CapsuleCollider>();
            _camera = GetComponentInChildren<Camera>();

            _collider.height = playerHeight;
            _collider.center = new Vector3(0, playerHeight * 0.5f, 0);

            _characterController.height = playerHeight;
            _characterController.center = new Vector3(0, playerHeight * 0.5f, 0);

            _camera.transform.localPosition = new Vector3(0, playerHeight, 0);

            Cursor.lockState = CursorLockMode.Locked;
            currentSpeed = walkSpeed;

            cameraDefaultLocalPos = _camera.transform.localPosition;

            playerInput = GetComponent<InputManager>();
        }

        void Update()
        {
            Move();
            Look();
            ApplyViewBobbing();
        }

        private void Move()
        {
            Vector2 movementVector = playerInput.ReadMovementInput();
            float horizontal = movementVector.x;
            float vertical = movementVector.y;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 movDir = forward * vertical + right * horizontal;
            Vector3 move = movDir * currentSpeed;

            move.y -= gravity;

            _characterController.Move(move * Time.deltaTime);
        }

        private void Look()
        {
            Vector2 mouseInput = playerInput.ReadLookInput();

            float mouseX = mouseInput.x * sensitivity;
            float mouseY = mouseInput.y * sensitivity;

            float multiplier = 1f;
            yRotation += mouseX * multiplier;
            xRotation -= mouseY * multiplier;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }

        private void ApplyViewBobbing()
        {
            float speed = new Vector2(_characterController.velocity.x, _characterController.velocity.z).magnitude;

            if (speed > 0.1f && _characterController.isGrounded)
            {
                bobTimer += Time.deltaTime * (speed * bobFrequency * 0.5f);
                float newX = Mathf.Cos(bobTimer) * bobHorizontalAmplitude;
                float newY = Mathf.Sin(bobTimer * 2f) * bobVerticalAmplitude;

                Vector3 targetTargetPos = cameraDefaultLocalPos + new Vector3(newX, newY, 0);
                _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, targetTargetPos, Time.deltaTime * bobSmoothing);
            }
            else
            {
                bobTimer = 0;
                _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, cameraDefaultLocalPos, Time.deltaTime * bobSmoothing);
            }
        }

        public Vector3 GetPlayerVelocity()
        {
            return _characterController.velocity;
        }
    }
}