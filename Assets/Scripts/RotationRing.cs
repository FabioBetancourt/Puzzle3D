using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RotationRing : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] private float rotationAmount = 60.0f; // Amount to rotate the ring
    [SerializeField] private float rotationSpeed = 5.0f;   // Speed at which the ring rotates
    [SerializeField] private float dragThreshold = 10.0f;  // Minimum drag distance to trigger rotation
    [SerializeField] private Transform[] faces;             // Faces that need to be aligned for victory

    // State variables
    private Vector3 _startMousePosition;
    private Transform _selectedRing = null;
    private bool _isDragging = false;
    private int _movesLeft = 10;
    private bool _canInteract = false;
    
    // Cached References
    private PlayerInput _playerInput;
    private InputAction _dragAction;
    private InputAction _moveAction;
    private InputAction _selectRingAction;
    private UIController _uiController;
    private UIManager _uiManager;

    private void Awake()
    {
        // Cache input and UI components
        _playerInput = GetComponent<PlayerInput>();
        _dragAction = _playerInput.actions["Drag"];
        _moveAction = _playerInput.actions["Move"];
        _selectRingAction = _playerInput.actions["SelectRing"]; 
        _uiController = FindObjectOfType<UIController>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        // Bind drag, select ring and move events
        _dragAction.performed += OnDragPerformed;
        _dragAction.canceled += OnDragCanceled;
        _dragAction.Enable();

        _selectRingAction.performed += OnSelectRingPerformed;
        _selectRingAction.Enable();

        _moveAction.performed += OnMovePerformed;
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        // Unbind events to avoid memory leaks and unexpected behaviors
        _selectRingAction.performed -= OnSelectRingPerformed;
        _selectRingAction.Disable();
        _moveAction.performed -= OnMovePerformed;
        _moveAction.Disable();
    }

    private void OnSelectRingPerformed(InputAction.CallbackContext context)
    {
        if (!_canInteract) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // If a ring is selected, cache its transform
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Ring"))
            _selectedRing = hit.transform;
        else
            _selectedRing = null;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!_canInteract) return;

        float moveDirection = context.ReadValue<float>();

        // Rotate the selected ring based on the move direction
        if (_selectedRing)
        {
            if (moveDirection > 0)
            {
                StartCoroutine(RotateRingByAmount(_selectedRing, rotationAmount));
                _uiController.DecrementMoves();
            }
            else if (moveDirection < 0)
            {
                StartCoroutine(RotateRingByAmount(_selectedRing, -rotationAmount));
                _uiController.DecrementMoves();
            }
        }
    }

    private void OnDragPerformed(InputAction.CallbackContext context)
    {
        if (!_canInteract) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Initialize drag variables if a ring is hit by the ray
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Ring"))
        {
            _isDragging = true;
            _selectedRing = hit.transform;
            _startMousePosition = mousePosition;
        }
    }

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        if (!_canInteract) return;

        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        float dragDirection = currentMousePosition.x - _startMousePosition.x;

        // Determine drag direction based on mouse position relative to screen center
        bool isMouseBelowCenter = currentMousePosition.y < Screen.height / 2;
        if (isMouseBelowCenter)
            dragDirection *= -1;

        // Rotate ring if drag was significant
        if (Mathf.Abs(dragDirection) > dragThreshold)
        {
            StartCoroutine(RotateRingByAmount(_selectedRing, Mathf.Sign(dragDirection) * rotationAmount));
            _selectedRing = null;
            _uiController.DecrementMoves();
        }

        _isDragging = false;
    }

    // Coroutine for smooth ring rotation
    IEnumerator RotateRingByAmount(Transform ring, float amount)
    {
        Vector3 startRotation = ring.eulerAngles;
        Vector3 endRotation = new Vector3(ring.eulerAngles.x, ring.eulerAngles.y, ring.eulerAngles.z + amount);
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rotationSpeed;
            ring.eulerAngles = Vector3.Lerp(startRotation, endRotation, t);
            yield return null;
        }
        CheckVictoryCondition();
    }

    // Enable or disable player's ability to interact
    public void SetCanInteract(bool value)
    {
        _canInteract = value;
    }
    
    // Check if rings are aligned for a win condition
    private void CheckVictoryCondition()
    {
        foreach (Transform face in faces)
        {
            if (Mathf.Abs(face.eulerAngles.z) > 0.1f) 
                return;
        }
        _uiManager.ShowWin();
    }
}
