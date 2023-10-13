using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerHUD : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();

    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if(IsServer) 
        { 
            playerName.Value = $"Player {OwnerClientId}";
            
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playerName.Value;
    }

    private void Update()
    {
        if (!overlaySet && !string.IsNullOrEmpty(playerName.Value))
        {
            SetOverlay();
        }
    }
}
