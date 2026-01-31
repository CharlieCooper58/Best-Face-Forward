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
        lobbiesPanel.SetActive(true);
    }
}
