using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

public class PlayerDataManager : NetworkBehaviour
{
    public static PlayerDataManager instance;
    ISession session;
    Dictionary<string, PlayerData> playerDataDict;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        session = SessionManager.instance.ActiveSession;
        if (IsServer)
        {
            playerDataDict = new();
            var players = session.Players;
            foreach (var p in players)
            {
                var playerName = "Unknown";
                if (p.Properties.TryGetValue("playerName", out var playerNameProperty))
                    playerName = playerNameProperty.Value;
                PlayerData dat = new PlayerData(p.Id,playerName);
                playerDataDict[p.Id] = dat;
            }
        }
    }

    // Note: this isn't hooked up to anything yet.  Need to set up the RPC calls for this in ResponseManager
    public void RegisterPlayerResponses(string ID, string[] parts, string response)
    {
        playerDataDict[ID].SetResponses(parts, response);
    }
    public string[] GetResponse(string ID)
    {
        return playerDataDict[ID].GetResponse();
    }
}

class PlayerData
{
    public string PlayerID;
    public string PlayerName;
    public int score;

    public string eye;
    public string nose;
    public string mouth;

    public string response;
    public PlayerData(string ID, string Name)
    {
        PlayerID = ID;
        PlayerName = Name;
    }
    public void SetResponses(string[] features, string response)
    {
        eye = features[0];
        nose = features[1];
        mouth = features[2];
        this.response = response;
    }
    public void ResetResponses()
    {
        eye = "";
        nose = "";
        mouth = "";
        response = "";
    }
    public string[] GetResponse()
    {
        var responseArray = new string[]
        {
            eye,
            nose,
            mouth,
            response
        };
        return responseArray;
    }
}
