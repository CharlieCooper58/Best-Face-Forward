using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickableWord : MonoBehaviour
{
    [SerializeField] Button wordSwapButton;
    Transform wordBankTransform;
    Transform responseAreaTransform;
    string word;
    [SerializeField] TMP_Text wordText;

    bool isInWordBank;

    private void Start()
    {
        wordSwapButton.onClick.AddListener(() => SwapWordPosition());
        isInWordBank = true;
    }
    public void Setup(Transform bankTransform, Transform responseTransform, string word)
    {
        wordBankTransform = bankTransform;
        responseAreaTransform = responseTransform;
        this.word = word;
        wordText.text = word;
    }

    private void SwapWordPosition()
    {
        SoundEffectManager.instance.PlaySoundByName("DialogueSelect", 1.8f, 0.02f);
        if (isInWordBank)
        {
            transform.SetParent(responseAreaTransform, false);
            isInWordBank = false;
        }
        else
        {
            transform.SetParent(wordBankTransform, false);
            isInWordBank = true;
        }
    }

    public string GetWord()
    {
        return word;
    }
}
