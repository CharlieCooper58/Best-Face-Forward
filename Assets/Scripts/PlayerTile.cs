using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTile : MonoBehaviour
{
    [SerializeField] Image eyesImage;
    [SerializeField] Image noseImage;
    [SerializeField] Image mouthImage;
    [SerializeField] TMP_Text responseText;
    public Button voteButton;
    public string playerID;

    [SerializeField] Transform voteTabulationArea;
    [SerializeField] GameObject voteTally;

    AudioSource blingPlayer;

    private void Awake()
    {
        voteButton = GetComponentInChildren<Button>();
    }
    public void SetDetails(string ID, string eyesHash, string noseHash, string mouthHash, string response)
    {
        EnableButton();
        playerID = ID;
        eyesImage.sprite = InventoryManager.instance.GetSpriteFromPartName(eyesHash);
        noseImage.sprite = InventoryManager.instance.GetSpriteFromPartName(noseHash);
        mouthImage.sprite = InventoryManager.instance.GetSpriteFromPartName(mouthHash);
        responseText.text = response;
    }
    public void ResetVotes()
    {
        for(int i = voteTabulationArea.childCount-1; i >= 0; i--)
        {
            Destroy(voteTabulationArea.GetChild(i));
        }
    }
    public void AddVote()
    {
        SoundEffectManager.instance.PlaySoundByName("AddPoint");
        Instantiate(voteTally, voteTabulationArea);
    }

    public void DisableButton()
    {
        voteButton.enabled = false;
    }
    public void EnableButton()
    {
        voteButton.enabled = true;
    }
}
