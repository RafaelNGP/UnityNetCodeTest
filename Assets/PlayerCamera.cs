using UnityEngine;
using Unity.Netcode;

public class PlayerCamera : NetworkBehaviour
{
    // A camera attached to the player prefab    
    [SerializeField] private Camera _camera;

    // This method is called when the player object is spawned    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // Check if the player object is the local player        
        if (IsLocalPlayer)
        {
            // Enable the camera
            _camera.enabled = true;
        }
        else
        {
            // Disable the camera
            _camera.enabled = false;
        }
    }
}