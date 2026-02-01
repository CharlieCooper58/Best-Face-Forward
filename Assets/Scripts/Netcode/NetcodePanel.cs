using UnityEngine;
using UnityEngine.UI;

public class NetcodePanel : MonoBehaviour
{
    [SerializeField] Button playButton;

    [SerializeField] GameObject lobbiesPanel;

    private void Start()
    {
        playButton.onClick.AddListener(() => OpenLobbiesPanel());
    }

    private void OpenLobbiesPanel()
    {
        SoundEffectManager.instance.PlaySoundByName("UI_Confirm", 1.5f, .02f);

        lobbiesPanel.SetActive(true);
    }
}
