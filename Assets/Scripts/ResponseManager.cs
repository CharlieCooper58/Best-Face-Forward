using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;

public class ResponseManager : NetworkBehaviour
{
    public static ResponseManager instance;
    [SerializeField] private Part[] face_parts;
    [SerializeField] private List<string> prompt_response;
    [SerializeField] private GameObject response_parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] ThemeBank earlyThemes;
    [SerializeField] ThemeBank round3Themes;

    [SerializeField] TMP_Text themeText;
    [SerializeField] TMP_Text promptText;
    [SerializeField] GameObject contents;

    public string roundTheme;
    public string roundPrompt;

    bool roundIsActive;
    void Start()
    {
        instance = this;
        prompt_response = new List<string>();
        face_parts = new Part[3]; //Eye, nose, mouth
    }
    public void StartFacebuildingRound()
    {
        ChooseTheme();
        StartFaceBuildingRoundClientRPC(roundTheme, roundPrompt);
    }
    [Rpc(SendTo.ClientsAndHost)]
    public void StartFaceBuildingRoundClientRPC(string theme, string prompt)
    {
        SoundEffectManager.instance.PlayFaceMusic();
        SoundEffectManager.instance.PlaySoundByName("DX_ChoosePrompt", 1.2f);
        SetThemeAndPrompt(theme, prompt);
        InventoryManager.instance.DealNewHand();
        roundIsActive = true;
        contents.SetActive(true);
    }
    public void EndFaceBuildingRound()
    {
        TimesUpClientRPC();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void TimesUpClientRPC()
    {
        SoundEffectManager.instance.StopMusic();
        SoundEffectManager.instance.PlaySoundByName("FinishingWhistle", 0.8f);
        var partsResults = GetPartsResponse();
        var promptResults = GetResponse();
        StartCoroutine(HideContentsAfterDelay());
        SubmitResultsServerRPC(AuthenticationService.Instance.PlayerId, partsResults[0], partsResults[1], partsResults[2], promptResults);
        RoundManager.instance.ShowLoadingScreen();
    }
    IEnumerator HideContentsAfterDelay()
    {
        yield return new WaitForSeconds(1);
        contents.SetActive(false);

    }
    [Rpc(SendTo.Server)]
    public void SubmitResultsServerRPC(string ID, string eyes, string nose, string mouth, string response)
    {
        PlayerDataManager.instance.RegisterPlayerResponses(ID, new[] {eyes, nose, mouth}, response);
        RoundManager.instance.RegisterResponse();
    }
    private void ChooseTheme()
    {
        var rt = (RoundManager.instance.roundCount == 3 ? round3Themes.GetRandomTheme() : earlyThemes.GetRandomTheme());
        roundTheme = rt.theme;
        roundPrompt = rt.prompt;
    }
    public void SetThemeAndPrompt(string theme, string prompt)
    {
        themeText.text = theme;
        promptText.text = prompt;
        roundTheme = theme;
        roundPrompt = prompt;
    }
    public bool AddItem(Part new_part){
        if(face_parts[(int)new_part.GetPartType()] == null){
            face_parts[(int)new_part.GetPartType()] = new_part;
            return true;
        }
        return false;
    }
    public void RemoveItem(Part old_part){
        if(old_part.GetPartType() != FeatureType.word){
            face_parts[(int)old_part.GetPartType()] = null;
        } else {
            print("Haven't implemented removing words yet.");
            //RemoveWord();
            //prompt_response.Remove()
        }
    }
    /*public int AddWord(string s){
        int insert_idx = prompt_response.Count;
        prompt_response.Add(s);
        return insert_idx;
    }
    public void RemoveWord(int idx){
        prompt_response.RemoveAt(idx);
    }*/
    public string[] GetPartsResponse(){
        string[] ans = new string[face_parts.Length];
        for(int i = 0; i < ans.Length; i++){
            ans[i] = face_parts[i] != null ? face_parts[i].GetID() : "";
        }
        return ans;
    }
    /*public string GetResponse(){
        string ans = "";
        for(int i = 0; i < prompt_response.Count; i++){
            ans += prompt_response[i];
            ans += " ";
        }
        //Maybe add period but maybe it's not the emphasis you want
        return ans;
    }*/
    public string GetResponse(){
        string ans = "";
        for(int i = 0; i < response_parent.transform.childCount; i++){
            ans += response_parent.transform.GetChild(i).GetComponent<ClickableWord>().GetWord();
            ans += " ";
        }
        return ans;
    }
    public List<string> GetResponseList(){
        return prompt_response;
    }
}
