using System;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's more than one Player instance.");
        }

        Instance = this;
    }

    private void Start ()
    {
        gameInput.OnInteractAction += GameInput_InInteraction;
    }

    private void GameInput_InInteraction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();

    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;

        // if not moving then you are moving in some direction *For handle interaction
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                // Check if different
                // if this clearcounter is the one at front is different from the one selected
                if (clearCounter != selectedCounter)
                {
                    // then select the selected to this one
                    SetSelectedCounter(clearCounter);
                }
                else
                {
                    SetSelectedCounter(null);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
    }

    private void HandleMovement()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 0.2f;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        bool canMove = !Physics.CapsuleCast(
                                            transform.position,
                                            transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            moveDir,
                                            moveDistance
                                            );

        isWalking = moveDir != Vector3.zero;
        float rotatingSpeed = 10f;

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(
                                            transform.position,
                                            transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            moveDirX,
                                            moveDistance
                                            );
            if (canMove)
            {
                // Can move only X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(
                                            transform.position,
                                            transform.position + Vector3.up * playerHeight,
                                            playerRadius,
                                            moveDirZ,
                                            moveDistance
                                            );
                if (canMove)
                {
                    // Can move only Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }

        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatingSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
