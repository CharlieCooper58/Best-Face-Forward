using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWaitingRoom : MonoBehaviour
{
    ISession session;
    LobbyFaceIcon[] faceIcons;

    [SerializeField] Button leaveLobbyButton;
    [SerializeField] Button startGameButton;
    [SerializeField] string gameSceneName;

    [SerializeField] TMP_Text lobbyNameText;


    private void Start()
    {
        leaveLobbyButton.onClick.AddListener(() => LeaveLobby());
        startGameButton.onClick.AddListener(() => TryStartGame());
    }
    private void OnEnable()
    {
        faceIcons = GetComponentsInChildren<LobbyFaceIcon>();
        session = SessionManager.instance.ActiveSession;
        UpdateFaceIcons();

        lobbyNameText.text = session.Name;
        session.PlayerJoined += Session_PlayerJoined;
        session.PlayerHasLeft += Session_PlayerLeft;
        startGameButton.gameObject.SetActive(session.IsHost);
    }



    protected void OnDisable()
    {
        DisableAllFaceIcons();
    }

    private void LeaveLobby()
    {
        SessionManager.instance.LeaveSession();
        gameObject.SetActive(false);
    }

    private void Session_PlayerLeft(string obj)
    {
        OnPlayerLeftSession(obj);
    }

    private void Session_PlayerJoined(string obj)
    {
        OnPlayerJoinedSession(obj);
    }
    public void OnPlayerJoinedSession(string playerId)
    {
        UpdateFaceIcons();
    }

    public void OnPlayerLeftSession(string playerId)
    {
        UpdateFaceIcons();
    }

    public void UpdateFaceIcons()
    {
        var players = session.Players;
        for(int i = 0; i < faceIcons.Length; i++)
        {
            if(i > players.Count-1)
            {
                faceIcons[i].DisconnectPlayer();
                continue;
            }
            var player = session.Players[i];
            var playerID = player.Id;

            var playerName = "Unknown";
            if (player.Properties.TryGetValue("playerName", out var playerNameProperty))
                playerName = playerNameProperty.Value;
            bool isLocal = playerID.Equals(AuthenticationService.Instance.PlayerId);
            faceIcons[i].UpdatePlayerInfo(playerName, playerID, isLocal);
        }
        
    }
    public void DisableAllFaceIcons()
    {
        for(int i = 0; i< faceIcons.Length; i++)
        {
            faceIcons[i].DisconnectPlayer();
        }
    }

    public void TryStartGame()
    {
        // Change this back
        if(!session.IsHost || session.PlayerCount < 1)
        {
            return;
        }
        session.AsHost().IsLocked = true;
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
