using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResponseManager : MonoBehaviour
{
    public static ResponseManager rM;
    [SerializeField] private Part[] face_parts;
    [SerializeField] private List<string> prompt_response;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rM = this;
        prompt_response = new List<string>();
        face_parts = new Part[3]; //Eye, nose, mouth
        print(face_parts[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AddItem(Part new_part){
        /*for(int i = 0; i < face_parts.Length; i++){
            if(face_parts[i].GetPartType() == new_part.GetPartType()){
                return false;
            }
        }*/
        if(face_parts[(int)new_part.GetPartType()] == null){
            face_parts[(int)new_part.GetPartType()] = new_part;
            return true;
        }
        return false;
    }
    public void RemoveItem(Part old_part){
        face_parts[(int)old_part.GetPartType()] = null;
    }
    public int AddWord(string s){
        int insert_idx = prompt_response.Count;
        prompt_response.Add(s);
        return insert_idx;
    }
    public void RemoveWord(int idx){
        prompt_response.RemoveAt(idx);
    }
    public string[] GetPartsResponse(){
        string[] ans = new string[face_parts.Length];
        for(int i = 0; i < ans.Length; i++){
            ans[i] = face_parts[i].GetID();
        }
        return ans;
    }
    public string GetResponse(){
        string ans = "";
        for(int i = 0; i < prompt_response.Count; i++){
            ans += prompt_response[i];
            ans += " ";
        }
        //Maybe add period but maybe it's not the emphasis you want
        return ans;
    }
    public List<string> GetResponseList(){
        return prompt_response;
    }
}
