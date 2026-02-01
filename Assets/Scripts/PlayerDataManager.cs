using System.Collections.Generic;
using System.Linq;
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
    public string SerializeAllResponses()
    {
        return string.Join("||",
            playerDataDict.Select(kvp =>
                $"{kvp.Key}|{kvp.Value.eye}|{kvp.Value.nose}|{kvp.Value.mouth}|{kvp.Value.response}"
            )
        );
    }
    public void DeserializeVotingData(string data)
    {
        foreach (var entry in data.Split("||"))
        {
            var parts = entry.Split('|');
            var id = parts[0];
            if(!playerDataDict.TryGetValue(id, out var playerData))
            {
                playerDataDict[id] = new PlayerData(id, "Unknown");
            }
            playerDataDict[id].SetResponses(
                new[] { parts[1], parts[2], parts[3] },
                parts[4]
            );
        }
    }
    public void RegisterPlayerResponses(string ID, string[] parts, string response)
    {
        playerDataDict[ID].SetResponses(parts, response);
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
