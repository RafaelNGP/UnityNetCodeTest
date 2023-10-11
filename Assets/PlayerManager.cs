using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public static PlayerManager Instance { get; internal set; }

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"{id} just connected...");
                playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"{id} just disconnected...");
                playersInGame.Value--;
            }
        };
    }

    // Inicializa a propriedade Instance no construtor da classe
    public PlayerManager()
    {
        Instance = this;
    }
}
