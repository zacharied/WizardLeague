using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class NetworkedPlayer : NetworkBehaviour
{
    private const double GameSceneUnfreezeDelay = 1;
    
    public GameObject localPlayerPrefab;

    public NetworkObject network;
    
    public bool hasLoadedFieldScene { get; private set; } = false;

    void Start()
    {
        network = GetComponent<NetworkObject>();
        
        var status = GetComponent<PlayerStatus>();
        
        DontDestroyOnLoad(gameObject);
        
        Debug.Log($"Created NetworkedPlayer with ID {network.OwnerClientId}");
    }

    public void InitForGame(int playerNumber)
    {
        var status = GetComponent<PlayerStatus>();
        status.playerNumber = playerNumber;
    }

    public void SetFieldSceneLoaded()
    {
        if (IsHost) {
            SetFieldSceneLoaded_ClientRpc();
        }
        else {
            SetFieldSceneLoaded_ServerRpc();
            SetFieldSceneLoaded_Inner();
        }
    }
    
    [ClientRpc]
    private void SetFieldSceneLoaded_ClientRpc()
    {
        SetFieldSceneLoaded_Inner();
    }

    [ServerRpc]
    private void SetFieldSceneLoaded_ServerRpc()
    {
        SetFieldSceneLoaded_Inner();
    }

    private void SetFieldSceneLoaded_Inner()
    {
        hasLoadedFieldScene = true;
    }

    public void SynchronizeGameStart()
    {
        if (!IsHost) {
            FetchStartTime_ServerRpc(DateTime.Now);
        }
    }

    [ServerRpc(Delivery = RpcDelivery.Reliable)]
    private void FetchStartTime_ServerRpc(DateTime clientTime)
    {
        var now = DateTime.Now;
        
        if ((now - clientTime).TotalSeconds > GameSceneUnfreezeDelay / 2) {
            throw new Exception("ping too fuckin high!");
        }
        
        var matchStartTime = now + TimeSpan.FromSeconds(GameSceneUnfreezeDelay);
        FetchStartTime_ClientRpc(matchStartTime.Ticks);
    }
    
    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    private void FetchStartTime_ClientRpc(long resumeTimestampTicks)
    {
        // The client and server have agreed upon a time to start the match.
        // Wait until then and then resume physics.
        UniTask.Void(async () =>
        {
            var startTime = new DateTime(resumeTimestampTicks);
            Debug.Log($"(async) Waiting until {startTime}");
            await UniTask.WaitUntil(() =>
            {
                var now = DateTime.Now;
                return now > startTime;
            });
            Time.timeScale = 1;
        });
    }
}
