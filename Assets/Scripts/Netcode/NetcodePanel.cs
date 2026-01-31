using UnityEngine;
using UnityEngine.UI;

public class NetcodePanel : MonoBehaviour
{
    [SerializeField] Button hostGameButton;

    [SerializeField] GameObject createLobbyPanel;

    private void Start()
    {
        hostGameButton.onClick.AddListener(() => OpenCreateLobbyPanel());
    }

    private void OpenCreateLobbyPanel()
    {
        createLobbyPanel.SetActive(true);
    }
}
