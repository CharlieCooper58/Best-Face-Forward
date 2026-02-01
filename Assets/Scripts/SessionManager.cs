using UnityEngine;
using Unity.Services.Multiplayer;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    ISession activeSession;

    public ISession ActiveSession
    {
        get => activeSession;

        private set
        {
            activeSession = value;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync(); // Initialize gaming services
            await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Sign in anonymously
            Debug.Log($"Signed in!  ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task<Dictionary<string, PlayerProperty>> GetPlayerProperties()
    {
        // Add functionality for saving player name in playerPrefs
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        var nameProperty = new PlayerProperty(playerName, VisibilityPropertyOptions.Member);
        return new Dictionary<string, PlayerProperty> { { "playerName", nameProperty } };
    }
    public async Task StartSessionAsHost(string sessionName)
    {
        var playerProperties = await GetPlayerProperties();
        var options = new SessionOptions
        {
            Name = sessionName,
            MaxPlayers = 6,
            IsLocked = false,
            IsPrivate = false,
            PlayerProperties = playerProperties
        }.WithRelayNetwork();

        ActiveSession = await MultiplayerService.Instance.CreateSessionAsync(options);
    }

    public async Task JoinSessionByID(string sessionID)
    {
        var playerProperties = await GetPlayerProperties();
        var joinOptions = new JoinSessionOptions { PlayerProperties = playerProperties };
        ActiveSession = await MultiplayerService.Instance.JoinSessionByIdAsync(sessionID, joinOptions);
        Debug.Log($"Joined session {sessionID}");
    }
    public async Task JoinSessionByCode(string sessionCode)
    {
        var playerProperties = await GetPlayerProperties();
        var joinOptions = new JoinSessionOptions { PlayerProperties = playerProperties };
        ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionCode, joinOptions);
        Debug.Log($"Joined session by code {sessionCode}");
    }
    async Task KickPlayer(string playerID)
    {
        if (!activeSession.IsHost)
        {
            return;
        }
        await ActiveSession.AsHost().RemovePlayerAsync(playerID);
    }
    public async Task<IList<ISessionInfo>> QuerySessions()
    {
        var sessionFilters = new List<FilterOption>();
        sessionFilters.Add(new FilterOption(FilterField.IsLocked, "false", FilterOperation.Equal));
        var SessionQueryOptions = new QuerySessionsOptions { FilterOptions=sessionFilters};
        var results = await MultiplayerService.Instance.QuerySessionsAsync(SessionQueryOptions);
        return results.Sessions;
    }
    public IReadOnlyList<IReadOnlyPlayer> GetPlayersInLobby()
    {
        return ActiveSession.Players;
    }

    public async void LeaveSession()
    {
        try { await ActiveSession.LeaveAsync(); }
        catch { }
        finally
        {
            activeSession = null;
        }
    }

    public async void EndGame()
    {
        try { await ActiveSession.LeaveAsync(); }
        catch { }
        finally
        {
            activeSession = null;
        }
        SceneManager.LoadScene("Main Menu");
    }
}
