using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager instance;
    public ISession session;
    public NetworkVariable<int> playersConnected = new NetworkVariable<int>(0);
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
    public const float facebuildingRoundTimer = 90;
    public const float votingRoundTimer = 20;
    public const float resultsRoundTimer = 15;

    public override void OnNetworkSpawn()
    {
        instance = this;
        base.OnNetworkSpawn();
        RegisterPlayerConnectedServerRPC();
        session = SessionManager.instance.ActiveSession;
        currentRound = RoundName.waitForPlayers;
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void RegisterPlayerConnectedServerRPC()
    {
        playersConnected.Value++;
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
                if(playersConnected.Value == session.PlayerCount)
                {
                    currentRound = RoundName.facebuilding;
                    Debug.Log("All players connected!");
                    CloseLoadingScreenClientRPC();
                    ResponseManager.instance.StartFacebuildingRound();

                    timer.Value = facebuildingRoundTimer;
                }
                break;
            case RoundName.facebuilding:
                if(timer.Value <= 0)
                {
                    ResponseManager.instance.EndFaceBuildingRound();
                    currentRound = RoundName.voting;
                    timer.Value = votingRoundTimer;
                    VotingCanvas.StartVotingRound();
                    // To do: add animation showing time's up
                    // Add sound effect showing time's up
                }
                break;
            case RoundName.voting:
                if(timer.Value <= 0)
                {
                    VotingCanvas.FinalizeVoting();
                    currentRound = RoundName.tabulating;
                    ShowLoadingScreenClientRPC();
                }
                break;
            case RoundName.tabulating:
                if (VotingCanvas.votingComplete.Value)
                {
                    currentRound = RoundName.results;
                    CloseLoadingScreenClientRPC();
                }
                break;

        }
    }

    [Rpc(SendTo.Everyone)]
    public void CloseLoadingScreenClientRPC()
    {
        transitionCanvas.CloseLoadingScreen();
    }
    [Rpc(SendTo.Everyone)]
    public void ShowLoadingScreenClientRPC()
    {
        transitionCanvas.ShowLoadingScreen();
    }

    public int GetTimer()
    {
        return (int)timer.Value;
    }
}
