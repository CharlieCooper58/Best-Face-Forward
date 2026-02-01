using TMPro;
using UnityEngine;

public class SnarkyLoadingPanel : MonoBehaviour
{
    [SerializeField] TMP_Text snarkyCommentText;
    [SerializeField] string[] snarkyComments;
    private void OnEnable()
    {
        snarkyCommentText.text = snarkyComments[Random.Range(0, snarkyComments.Length)];
    }
}
