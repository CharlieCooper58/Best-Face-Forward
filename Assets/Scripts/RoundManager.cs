using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager instance;
    ISession session;
    public NetworkVariable<int> playersConnected = new NetworkVariable<int>(0);
    public int roundCount = 1;

    [SerializeField] GameObject LoadingCanvas;
    enum RoundName
    {
        waitForPlayers,
        tutorial,
        prompt,
        facebuilding,
        voting,
        results
    }
    RoundName currentRound;
    public const float facebuildingRoundTimer = 60;
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
        if (!IsHost)
        {
            return;
        }
        switch(currentRound)
        {
            case RoundName.waitForPlayers:
                if(playersConnected.Value == session.PlayerCount)
                {
                    currentRound = RoundName.facebuilding;
                    Debug.Log("All players connected!");
                    StartRoundOneClientRPC();
                }
                break;
            case RoundName.facebuilding:
                break;

        }
    }

    [Rpc(SendTo.Everyone)]
    public void StartRoundOneClientRPC()
    {
        LoadingCanvas.SetActive(false);
        ResponseManager.rM.StartFacebuildingRound();
    }
}
