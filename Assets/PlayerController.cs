using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{

    public enum PlayerState
    {
        Idle,
        Walk
    }

    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private Vector2 defaultInitialPlanePosition = new Vector2(-4, 4);
    [SerializeField] private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();
    [SerializeField] private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();
    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    // Client caching
    private Vector3 oldInputPosition;
    private Vector3 oldInputRotation;

    private CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y), 0, Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y));        
        }
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

         ClientMoveAndRotate();
         ClientVisuals();
    }

    private void ClientInput()
    {
        // Player position and rotation input
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Vector3 inputPosition = direction * forwardInput;

        if(oldInputPosition != inputPosition || oldInputRotation != inputRotation) 
        {
            oldInputRotation = inputRotation;
            oldInputPosition = inputPosition;
            UpdateClientPositionRotationServerRpc(inputPosition * speed, inputRotation * rotationSpeed);
        }

        // Player state changes based on input
        if(forwardInput >  0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
    }

    private void ClientMoveAndRotate()
    {
        if(networkPositionDirection.Value != Vector3.zero)
        {
            Debug.Log("Entrou no if do movimento");
            characterController.SimpleMove(networkPositionDirection.Value);
        } 
        
        if(networkRotationDirection.Value != Vector3.zero)
        {
            transform.Rotate(networkRotationDirection.Value);
        }
    }

    private void ClientVisuals()
    {
        if (networkPlayerState.Value == PlayerState.Walk)
        {
            animator.SetFloat("Walk", 1);
        } else
        {
            animator.SetFloat("Walk", 0);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionRotationServerRpc(Vector3 newPositionDirection, Vector3 newRotationDirection)
    {
        networkPositionDirection.Value = newPositionDirection;
        networkRotationDirection.Value = newRotationDirection;
    }

    [ServerRpc]

    private void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        networkPlayerState.Value = newState;
    }
}
