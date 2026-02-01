using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostGamePanel : MonoBehaviour
{
    [SerializeField] Button createLobbyButton;
    [SerializeField] TMP_InputField lobbyNameField;
    [SerializeField] GameObject inLobbyPanel;

    private void Start()
    {
        createLobbyButton.onClick.AddListener(StartLobby);
    }

    private async void StartLobby()
    {
        if(lobbyNameField.text == "")
        {
            return;
        }
        SoundEffectManager.instance.PlaySoundByName("UI_Confirm", 1.5f, .02f);
        await SessionManager.instance.StartSessionAsHost(lobbyNameField.text);
        lobbyNameField.text = string.Empty;
        inLobbyPanel.gameObject.SetActive(true);
    }
}
