using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ResponseManager : MonoBehaviour
{
    public static ResponseManager rM;
    [SerializeField] private Part[] face_parts;
    [SerializeField] private List<string> prompt_response;
    [SerializeField] private GameObject response_parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rM = this;
        prompt_response = new List<string>();
        face_parts = new Part[3]; //Eye, nose, mouth
    }

    // Update is called once per frame
    void Update()
    {
        
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
            ans[i] = face_parts[i].GetID();
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
            ans += response_parent.transform.GetChild(i).GetComponent<DraggableWord>().GetPart().GetID();
        }
        return ans;
    }
    public List<string> GetResponseList(){
        return prompt_response;
    }
}
