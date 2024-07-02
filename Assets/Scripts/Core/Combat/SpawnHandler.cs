using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Player playerPrefab;

    public override void OnNetworkSpawn()
    {
        DebugConsoleManager.Log("SpawnHandler Network Spawned!");
        if (!IsServer) { return; }
        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (Player player in players)
        {
            HandlePlayerSpawned(player);
        }
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePlayerDespawned;
    }

    private void HandleClientConnected(ulong clientId)
    {
        DebugConsoleManager.Log($"New client connected: {clientId}");
        SendLevelToClientServerRpc(clientId);
    }

    [ClientRpc]
    private void LoadActiveLevelClientRpc(Level level, ClientRpcParams clientRpcParams = default)
    {
        DebugConsoleManager.Log("Client RPC Received!");
        var saveTime = level.saveTime;
        DebugConsoleManager.Log("Active Level Save Time: " + saveTime);
    }

    [ServerRpc]
    private void SendLevelToClientServerRpc(ulong clientId)
    {
        Level activeLevel = LevelManager.GetInstance().GetActiveLevel();
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        LoadActiveLevelClientRpc(activeLevel, clientRpcParams);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePlayerDespawned;
    }


    private void HandlePlayerSpawned(Player player)
    {
        player.Health.DeathEvent += (ignored) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDespawned(Player player)
    {
        player.Health.DeathEvent -= (ignored) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDeath(Player player)
    {
        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }



    private IEnumerator RespawnPlayer(ulong OwnerClientId)
    {
        yield return null;
        Player playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPoint(), Quaternion.identity);
        playerInstance.NetworkObject.SpawnAsPlayerObject(OwnerClientId);
    }

}
