using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyFaceIcon : MonoBehaviour
{
    bool playerIsConnected;
    [SerializeField] TMP_Text thisIsYouText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image playerNotConnectedImage;
    [SerializeField] Image playerConnectedImage;

    string playerID;
    string playerName;

    public void UpdatePlayerInfo(string name, string playerID, bool isLocal)
    {
        playerName = name;
        this.playerID = playerID;
        playerConnectedImage.gameObject.SetActive(true);
        playerNotConnectedImage.gameObject.SetActive(false);
        nameText.text = playerName;
        thisIsYouText.gameObject.SetActive(isLocal);
    }

    public void DisconnectPlayer()
    {
        playerID = string.Empty;
        playerName = string.Empty;
        nameText.text = "Not Connected";
        playerNotConnectedImage.gameObject.SetActive(true);
        playerConnectedImage.gameObject.SetActive(false);
        thisIsYouText.gameObject.SetActive(false);
    }
}
