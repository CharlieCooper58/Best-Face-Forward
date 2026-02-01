using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTile : MonoBehaviour
{
    [SerializeField] Image eyesImage;
    [SerializeField] Image noseImage;
    [SerializeField] Image mouthImage;
    [SerializeField] TMP_Text responseText;

    string playerID;

    public void SetDetails(string ID, string eyesHash, string noseHash, string mouthHash, string repsonse)
    {
        
    }
}
