using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CreateLobbyPanel : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Button createButton;
    [SerializeField] TMP_InputField lobbyNameInput;

    private void Start()
    {
        createButton.onClick.AddListener(() => CreateLobby());
        closeButton.onClick.AddListener(() => ClosePanel());
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    private async Task CreateLobby()
    {
        string lobbyName = lobbyNameInput.text;
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
    }
}
