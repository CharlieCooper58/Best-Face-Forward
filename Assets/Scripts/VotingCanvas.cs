using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class VotingCanvas : NetworkBehaviour
{
    RoundManager roundManager;
    RoundManager RoundManager
    {
        get
        {
            if(roundManager == null)
            {
                roundManager = RoundManager.instance;
            }
            return roundManager;
        }
    }
    [SerializeField] TMP_Text themeText;
    [SerializeField] TMP_Text promptText;

    [SerializeField] PlayerTile playerTilePrefab; 

    PlayerTile[] tileChildren;
    Dictionary<string, PlayerTile> tilesDict;
    [SerializeField] Transform tileChildrenParent;

    [SerializeField] GameObject content;

    bool voteLocked;
    string currentVote = "";
    List<string> votes;
    int resultsReceived;
    int expectedVotes;
    public NetworkVariable<bool> votingComplete = new NetworkVariable<bool>(false);

    [SerializeField] GameObject timer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        tilesDict = new Dictionary<string, PlayerTile>();
    }
    public void StartVotingRound()
    {
        votingComplete.Value = false;
        StartVotingRoundClientRPC(PlayerDataManager.instance.SerializeAllResponses());
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void StartVotingRoundClientRPC(string serializedData)
    {
        SoundEffectManager.instance.PlayVotingMusic();
        PlayerDataManager.instance.DeserializePlayerResponse(serializedData);
        content.SetActive(true);
        currentVote = "";
        var players = RoundManager.session.Players;
        themeText.text = ResponseManager.instance.roundTheme;
        promptText.text = ResponseManager.instance.roundPrompt;
        if (tileChildren == null)
        {
            tileChildren = new PlayerTile[players.Count];
            for(int i = 0; i < tileChildren.Length; i++)
            {
                tileChildren[i] = Instantiate(playerTilePrefab, tileChildrenParent);
            }
        }
        tilesDict.Clear();
        for(int i = 0; i< tileChildren.Length; ++i)
        {
            tileChildren[i].ResetVotes();
            var response = PlayerDataManager.instance.GetResponse(players[i].Id);
            tileChildren[i].SetDetails(players[i].Id, response[0], response[1], response[2], response[3]);
            var ID = tileChildren[i].playerID;
            tileChildren[i].voteButton.onClick.RemoveAllListeners();
            tileChildren[i].voteButton.onClick.AddListener(() =>
            {
                SetVote(ID);
                SoundEffectManager.instance.PlaySoundByName("UI_Confirm", 1.5f, .02f);
            });
            tilesDict.Add(ID, tileChildren[i]);
        }
        timer.SetActive(true);
    }

    public void SetVote(string ID)
    {
        if (voteLocked)
        {
            return;
        }
        else
        {
            currentVote = ID;
        }
    }

    // Should only be called on the server
    public void FinalizeVoting()
    {
        votes = new List<string>();
        resultsReceived = 0;
        expectedVotes = SessionManager.instance.ActiveSession.PlayerCount;
        FinalizeVotingClientRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void FinalizeVotingClientRPC()
    {
        SoundEffectManager.instance.StopMusic();
        SoundEffectManager.instance.PlaySoundByName("FinishingWhistle", 0.8f);
        foreach (var playerTile in tileChildren)
        {
            playerTile.DisableButton();
        }
        SendVoteServerRPC(currentVote);
        timer.SetActive(false);
    }
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void SendVoteServerRPC(string vote)
    {
        if(vote != "")
        {
            votes.Add(vote);
            Debug.Log(votes);
        }
        RoundManager.instance.RegisterResponse();
    }

    public void CompleteVoting()
    {
        Debug.Log("Voting complete!");
        votingComplete.Value = true;
        for(int i = 0; i < votes.Count; i++)
        {
            PlayerDataManager.instance.AwardPoint(votes[i]);
        }
        DisplayVotingResultsClientRPC(string.Join("|", votes.ToArray()));
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void DisplayVotingResultsClientRPC(string votesSerialized)
    {
        votes = votesSerialized.Split('|').ToList();
        StartCoroutine(DisplayVotes());
        Debug.Log("Displaying votes!");
    }
    IEnumerator DisplayVotes()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < votes.Count; i++)
        {
            if(tilesDict.TryGetValue(votes[i], out var tile))
            {
                tile.AddVote();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
