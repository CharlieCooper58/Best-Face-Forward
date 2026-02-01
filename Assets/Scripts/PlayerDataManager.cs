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
        instance = this;
        session = SessionManager.instance.ActiveSession;

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
    
    public void RegisterPlayerResponses(string ID, string[] parts, string response)
    {
        RegisterPlayerResponseClientRPC(ID, parts[0], parts[1], parts[2], response);
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void RegisterPlayerResponseClientRPC(string ID, string eye, string nose, string mouth, string response)
    {
        playerDataDict[ID].SetResponses(new string[] {eye, nose, mouth}, response);
    }
    public string[] GetResponse(string ID)
    {
        return playerDataDict[ID].GetResponse();
    }
    public void AwardPoint(string ID)
    {
        playerDataDict[ID].AddPoint();
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
    public void AddPoint()
    {
        score++;
    }
}
