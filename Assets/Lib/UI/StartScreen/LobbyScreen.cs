using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyScreen : NetworkBehaviour
{
    private VisualElement root;
    
    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>("StartButton");
        button.clicked += () => GoToGame_ClientRpc();
    }

    private void Update()
    {
        var players = FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None);
        foreach (var player in players) {
            if (player.network.OwnerClientId == 0) {
                // Host is connected, assign to P1
                root.Q("Player1Side").RemoveFromClassList("unconnectedPlayerBg");
                root.Q("Player1Side").AddToClassList("connectedPlayerBg");
            }
            else {
                root.Q("Player2Side").RemoveFromClassList("unconnectedPlayerBg");
                root.Q("Player2Side").AddToClassList("connectedPlayerBg");

                if (player.network.IsLocalPlayer) {
                    var button = root.Q<Button>("StartButton");
                    button.text = "Waiting for host...";
                    button.SetEnabled(false);
                }
            }
        }
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    private void GoToGame_ClientRpc()
    {
        Debug.Log("RPC Call!");
        // Assign player numbers
        foreach (var np in FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None)) {
            np.InitForGame(np.network.OwnerClientId == 0 ? 1 : 2);
        }
        
        SceneManager.LoadScene("GameScene");
        Debug.Log("Pausing physics!");
        Time.timeScale = 0;
    }
}