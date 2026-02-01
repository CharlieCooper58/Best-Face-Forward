using Unity.Netcode;
using UnityEngine;

public class FacebuildingManager : NetworkBehaviour
{
    public static FacebuildingManager instance;

    [SerializeField] ThemeBank earlyThemes;
    [SerializeField] ThemeBank round3Themes;

    string roundTheme;
    string roundPrompt;

    bool roundIsActive;

    float timer;
    public void StartFacebuildingRound()
    {
        RandomizeFaceParts();
        RandomizeWordbanks();
        ChooseTheme();
        roundIsActive = true;
        timer = RoundManager.facebuildingRoundTimer;
    }
    private void Update()
    {
        if(IsServer && roundIsActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                TimesUpClientRPC();
            }
        }
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void TimesUpClientRPC()
    {

    }

    public void RandomizeFaceParts() { }
    public void RandomizeWordbanks() { }
    private void ChooseTheme()
    {
        var rt = (RoundManager.instance.roundCount == 3? round3Themes.GetRandomTheme():earlyThemes.GetRandomTheme());
        roundTheme = rt.theme;
        roundPrompt = rt.prompt;
    }
}
