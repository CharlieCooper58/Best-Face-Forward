using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoinPanel : MonoBehaviour
{
    [SerializeField] Transform lobbyScrollArea;
    [SerializeField] LobbyJoinButton lobbyJoinButtonPrefab;

    [SerializeField] Button refreshButton;
    LobbyJoinButton[] joinButtons;

    [SerializeField] TMP_Text joinFailedText;

    [SerializeField] GameObject inLobbyPanel;

    private void Start()
    {
        refreshButton.onClick.AddListener(() => _ = RefreshLobbies());
    }
    async void OnEnable()
    {
        await RefreshLobbies();
    }

    public async void OnLobbyJoinFailed()
    {
        joinFailedText.gameObject.SetActive(true);
        await RefreshLobbies();
    }
    public void OnLobbyJoinSuccess()
    {
        inLobbyPanel.gameObject.SetActive(true);

    }
    public async Task RefreshLobbies()
    {
        if (joinButtons != null)
        {
            for (int i = 0; i < joinButtons.Length; i++)
            {
                Destroy(joinButtons[i].gameObject);
            }
        }
        var sessions = await SessionManager.instance.QuerySessions();
        joinButtons = new LobbyJoinButton[sessions.Count];
        for (int i = 0; i < joinButtons.Length; ++i)
        {
            joinButtons[i] = Instantiate(lobbyJoinButtonPrefab, lobbyScrollArea);
            joinButtons[i].Initialize(this, sessions[i]);
        }
    }
}
