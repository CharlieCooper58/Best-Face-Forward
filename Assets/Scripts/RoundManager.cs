using System.Collections;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager instance;
    public ISession session;
    public NetworkVariable<int> playerResponses = new NetworkVariable<int>(0);
    int expectedResponses;
    bool forceContinue;
    public int roundCount = 1;

    [SerializeField] GameObject LoadingCanvas;
    [SerializeField] VotingCanvas VotingCanvas;

    [SerializeField] TransitionCanvas transitionCanvas;


    public NetworkVariable<float> timer = new NetworkVariable<float>(0f);
    enum RoundName
    {
        waitForPlayers,
        tutorial,
        prompt,
        facebuilding,
        voting,
        tabulating,
        results
    }
    RoundName currentRound;

    RoundName nextRound;
    public const float facebuildingRoundTimer = 60;
    public const float votingRoundTimer = 35;
    public const float resultsRoundTimer = 15;

    public override void OnNetworkSpawn()
    {
        instance = this;
        base.OnNetworkSpawn();
        RegisterPlayerConnectedServerRPC();
        session = SessionManager.instance.ActiveSession;
        currentRound = RoundName.waitForPlayers;
        nextRound = RoundName.facebuilding;
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void RegisterPlayerConnectedServerRPC()
    {
        playerResponses.Value++;
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        timer.Value -= Time.deltaTime;
        switch(currentRound)
        {
            case RoundName.waitForPlayers:
                if(forceContinue || playerResponses.Value == session.PlayerCount)
                {
                    forceContinue = false;
                    Debug.Log("All players connected!");
                    CloseLoadingScreenClientRPC();

                    if(nextRound == RoundName.facebuilding)
                    {
                        currentRound = RoundName.facebuilding;
                        ResponseManager.instance.StartFacebuildingRound();
                        timer.Value = facebuildingRoundTimer;
                    }
                    else if(nextRound == RoundName.voting)
                    {
                        currentRound = RoundName.voting;
                        VotingCanvas.StartVotingRound();
                        timer.Value = votingRoundTimer;
                    }
                    else if(nextRound == RoundName.tabulating)
                    {
                        currentRound = RoundName.tabulating;
                        VotingCanvas.CompleteVoting();
                    }
                }
                break;
            case RoundName.facebuilding:
                if(timer.Value <= 0)
                {
                    ResponseManager.instance.EndFaceBuildingRound();
                    currentRound = RoundName.waitForPlayers;
                    nextRound = RoundName.voting;
                    playerResponses.Value = 0;
                    StartCoroutine(WaitforPlayerResponses());
                    ShowLoadingScreenClientRPC();
                    //timer.Value = votingRoundTimer;
                    // To do: add animation showing time's up
                    // Add sound effect showing time's up
                }
                break;
            case RoundName.voting:
                if(timer.Value <= 0)
                {
                    VotingCanvas.FinalizeVoting();
                    currentRound = RoundName.waitForPlayers;
                    playerResponses.Value = 0;
                    StartCoroutine(WaitforPlayerResponses());
                    ShowLoadingScreenClientRPC();
                }
                break;
            case RoundName.tabulating:
                if (VotingCanvas.votingComplete.Value)
                {
                    currentRound = RoundName.results;
                    //CloseLoadingScreenClientRPC();
                }
                break;

        }
    }

    [Rpc(SendTo.Everyone)]
    public void CloseLoadingScreenClientRPC()
    {
        CloseLoadingScreen();
    }
    [Rpc(SendTo.Everyone)]
    public void ShowLoadingScreenClientRPC()
    {
        ShowLoadingScreen();
    }

    public void CloseLoadingScreen()
    {
        transitionCanvas.CloseLoadingScreen();
    }
    public void ShowLoadingScreen()
    {
        transitionCanvas.ShowLoadingScreen();
    }

    public int GetTimer()
    {
        return (int)timer.Value;
    }

    IEnumerator WaitforPlayerResponses()
    {
        playerResponses.Value = 0;
        expectedResponses = session.PlayerCount;
        yield return new WaitForSeconds(10);
        if (playerResponses.Value < expectedResponses)
        {
            forceContinue = true;
        }
    }
    public void RegisterResponse()
    {
        playerResponses.Value++;
    }
}
