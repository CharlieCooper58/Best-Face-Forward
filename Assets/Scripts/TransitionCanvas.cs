using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] GameObject tabulatingVotesPanel;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    const string voteEndAnimName = "EndVoting";
    const string tabulationEndAnimName = "TabulationEnd";
    public enum TransitionAnimation
    {
        voteEnd,
        tabulationEnd
    }
    public void PlayTransitionAnimation(TransitionAnimation anim)
    {
        switch(anim)
        {
            case TransitionAnimation.voteEnd:
                m_animator.Play(voteEndAnimName);
                break;
            case TransitionAnimation.tabulationEnd:
                m_animator.Play(tabulationEndAnimName);
                break;
        }
    }

    public void ShowLoadingScreen()
    {
        m_animator.Play("PlayLoadingScreen");
    }
    public void CloseLoadingScreen()
    {
        m_animator.Play("CloseLoadingScreen");
    }
}
