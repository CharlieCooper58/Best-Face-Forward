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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        tilesDict = new Dictionary<string, PlayerTile>();
    }
    public void StartVotingRound()
    {
        votingComplete.Value = false;
        StartVotingRoundClientRPC();
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void StartVotingRoundClientRPC()
    {
        content.SetActive(true);
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
            tileChildren[i].voteButton.onClick.AddListener(() => SetVote(ID));
            tilesDict.Add(ID, tileChildren[i]);
        }
        
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
        StartCoroutine(CompleteVotingAfterWait());
    }
    IEnumerator CompleteVotingAfterWait()
    {
        yield return new WaitForSeconds(10);
        if(resultsReceived < expectedVotes)
        {
            CompleteVoting();
        }
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void FinalizeVotingClientRPC()
    {
        SendVoteServerRPC(currentVote);
    }
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void SendVoteServerRPC(string vote)
    {
        if(vote != "")
        {
            votes.Add(vote);
        }
        resultsReceived++;
        TryCompleteVoting();
    }

    public void TryCompleteVoting()
    {
        if(resultsReceived == expectedVotes)
        {
            CompleteVoting();
        }
    }
    void CompleteVoting()
    {
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
    }
    IEnumerator DisplayVotes()
    {
        for (int i = 0; i < votes.Count; i++)
        {
            if(tilesDict.TryGetValue(votes[i], out var tile))
            {
                tile.AddVote();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
