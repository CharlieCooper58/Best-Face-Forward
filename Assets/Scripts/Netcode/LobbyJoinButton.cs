using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoinButton : MonoBehaviour
{
    [SerializeField] TMP_Text lobbyNameText;
    [SerializeField] TMP_Text numPlayersText;
    [SerializeField] Button joinButton;
    ISessionInfo mySession;

    LobbyJoinPanel parent;

    public void Initialize(LobbyJoinPanel parent, ISessionInfo newSession)
    {
        this.parent = parent;
        joinButton.onClick.AddListener(() => _ = JoinLobby());
        lobbyNameText.text = newSession.Name;
        mySession = newSession;
    }

    async Task JoinLobby()
    {
        try
        {
            await SessionManager.instance.JoinSessionByID(mySession.Id);
            parent.OnLobbyJoinSuccess();
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            parent.OnLobbyJoinFailed();
        }
    }
}
