using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UIElements;

public class StartScreen : MonoBehaviour
{
    public VisualTreeAsset LobbyForm;
    
    RadioButton hostButton;
    RadioButton clientButton;
    TextField addressField;
    Label connectingNoticeLabel;
    private Label errorNoticeLabel;

    private bool isConnecting;

    void Start()
    {
        var document = GetComponent<UIDocument>();
        var root = document.rootVisualElement;
        
        hostButton = root.Q<RadioButton>("HostButton");
        clientButton = root.Q<RadioButton>("ClientButton");
        addressField = root.Q<TextField>("IpAddressField");
        connectingNoticeLabel = root.Q<Label>("ConnectingNoticeLabel");
        errorNoticeLabel = root.Q<Label>("ErrorNoticeLabel");

        var network = FindObjectOfType<NetworkManager>();
        var transport = FindObjectOfType<UnityTransport>();

        root.Q<Button>("StartButton").clicked += () =>
        {
            if (hostButton.value) {
                network.StartHost();
                GoToLobby();
            } else if (clientButton.value) {
                transport.SetConnectionData(addressField.value, 7777);
                network.OnClientDisconnectCallback += OnDisconnect;
                network.OnClientConnectedCallback += id => GoToLobby();
                network.StartClient();

                isConnecting = true;
            }
        };

        network.OnClientConnectedCallback += (id) =>
        {
            Debug.Log($"Client {id} connected!");
        };
    }

    private void OnDisconnect(ulong obj)
    {
        Debug.Log("OnDisconnect");
        isConnecting = false;
        errorNoticeLabel.visible = true;
        UniTask.Void(async () =>
        {
            Debug.Log("Async!");
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            errorNoticeLabel.visible = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        connectingNoticeLabel.visible = isConnecting;
        hostButton.SetEnabled(!isConnecting);
        clientButton.SetEnabled(!isConnecting);
        
        if (hostButton.value || isConnecting) {
            addressField.SetEnabled(false);
        } else {
            addressField.SetEnabled(true);
        }
    }

    private void GoToLobby()
    {
        var document = GetComponent<UIDocument>();
        document.visualTreeAsset = LobbyForm;
        GetComponent<LobbyScreen>().enabled = true;
//        gameObject.AddComponent<LobbyScreen>();
        Destroy(this);
    }
}
