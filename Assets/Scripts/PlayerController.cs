using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(NetworkTransform))]

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
    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

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

         ClientVisuals();
    }

    private void ClientInput()
    {
        // Player position and rotation input
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);

        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Vector3 inputPosition = direction * forwardInput;

        characterController.SimpleMove(inputPosition * speed);
        transform.Rotate(inputRotation * rotationSpeed, Space.World);

        // Player state changes based on input
        if (forwardInput >  0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
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
    private void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        networkPlayerState.Value = newState;
    }
}
