using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text winnerText;
    [SerializeField] Button leaveGameButton;
    private void Start()
    {
        leaveGameButton.onClick.AddListener(ReturnToLobby);
    }
    public void DeclareWinner(string winnerName)
    {
        winnerText.text = winnerName;
    }

    void ReturnToLobby()
    {
        SessionManager.instance.EndGame();
    }
}
