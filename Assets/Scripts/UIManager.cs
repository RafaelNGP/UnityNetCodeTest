using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startServerButton;
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] private TMP_InputField codeArea;
    [SerializeField] private Button physicsButton;
    [SerializeField] TextMeshProUGUI playerInGameText;

    [SerializeField] private SpawnerController spawnerController;
    [SerializeField] private RelayManager relayManager;

    private bool hasServerStarted = false;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        playerInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(async () =>
        {
            if (relayManager.IsRelayEnabled)
            {
                await relayManager.SetupRelay();
                hideUI();
            }

            if (NetworkManager.Singleton.StartHost()){
                Debug.Log("Host started...");
            } else{
                Debug.LogError("Host was not started!");
            }
        });

        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer()){
                Debug.Log("Server started...");
            } else{
                Debug.LogError("Server was not started!");
            }
        });

        startClientButton.onClick.AddListener(async() =>
        {
            if (relayManager.IsRelayEnabled && !string.IsNullOrEmpty(codeArea.text))
            {
                await relayManager.JoinRelay(codeArea.text);
                hideUI();
            }

            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started...");
            } else{
                Debug.LogError("Client was not started!");
            }
        });

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };

        physicsButton.onClick.AddListener(() =>
        {
            if (!hasServerStarted) return;

            spawnerController.LaunchObjects();
        });
    }

    public void hideUI()
    {
       startServerButton.gameObject.SetActive(false);
       startHostButton.gameObject.SetActive(false);
       startClientButton.gameObject.SetActive(false);
       codeArea.gameObject.SetActive(false);
    }
}
