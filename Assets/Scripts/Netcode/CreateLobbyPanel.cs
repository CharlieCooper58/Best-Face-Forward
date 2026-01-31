using TMPro;
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
        createButton.onClick.AddListener(CreateLobby);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    private void CreateLobby()
    {
        
    }
}
