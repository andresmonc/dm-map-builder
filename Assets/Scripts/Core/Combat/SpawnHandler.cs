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
    // [Header("Settings")]
    // [SerializeField] private float keptCoinPercentage = 50f;

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
        Debug.Log($"New client connected: {clientId}");
        DebugConsoleManager.Log($"New client connected: {clientId}");
        // Create ClientRpcParams to target the specific client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        // Call the ClientRPC method on the newly connected client
        LoadActiveLevelClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void LoadActiveLevelClientRpc(ClientRpcParams clientRpcParams = default)
    {
        DebugConsoleManager.Log("Client RPC Received!");
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
