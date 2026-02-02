using System.Collections.Generic;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] int num_each_item;
    [SerializeField] int num_words;
    [SerializeField] private Part[] allPartsList;
    private Part[] eye_scriptables;
    private Part[] nose_scriptables;
    private Part[] mouth_scriptables;
    Dictionary<string, Part> partsDictionary = new Dictionary<string, Part>();
    [SerializeField] private string[] word_list;

    private Part[] eye_player_scriptables;
    private Part[] nose_player_scriptables;
    private Part[] mouth_player_scriptables;
    private string[] rand_player_words;
    [SerializeField] private GameObject img_part_prefab;
    [SerializeField] private ClickableWord word_part_prefab;

    [SerializeField] private Transform eye_parent_t;
    [SerializeField] private Transform nose_parent_t;
    [SerializeField] private Transform mouth_parent_t;
    [SerializeField] private Transform wordBank;
    [SerializeField] private Transform wordResponseArea;

    [SerializeField] private Image eyesDropPoint;
    [SerializeField] private Image noseDropPoint;
    [SerializeField] private Image mouthDropPoint;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Part> eyeParts = new List<Part>();
        List<Part> noseParts = new List<Part>();
        List<Part> mouthParts = new List<Part>();
        for (int i = 0; i<allPartsList.Length; i++)
        {
            var id = allPartsList[i].GetID();
            if (!string.IsNullOrEmpty(id) && !partsDictionary.TryGetValue(id, out var p))
            {
                partsDictionary.Add(id, allPartsList[i]);

            }
            switch (allPartsList[i].type)
            {
                case FeatureType.eyes:
                    eyeParts.Add(allPartsList[i]);
                    break;                
                case FeatureType.nose:
                    noseParts.Add(allPartsList[i]);
                    break;                
                case FeatureType.mouth:
                    mouthParts.Add(allPartsList[i]);
                    break;
                
            }
        }
        eye_scriptables = eyeParts.ToArray();
        nose_scriptables = noseParts.ToArray();
        mouth_scriptables = mouthParts.ToArray();

        for(int i = 0; i < num_each_item; i++){
            Instantiate(img_part_prefab, eye_parent_t);
            Instantiate(img_part_prefab, nose_parent_t);
            Instantiate(img_part_prefab, mouth_parent_t);
        }
        word_list = WordListHelper.MergeRawWordLists();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Part[] Shuffle(Part[] list)
    {
        int n = list.Length;
        while (n > 1) {
            n--;
            int k = Random.Range(0,n);
            Part value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
    public string[] ShuffleWords(string[] list){
        int n = list.Length;
        while (n > 1) {
            n--;
            int k = Random.Range(0,n);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
    public void DealNewHand(){
        //Select Scriptable objects
        eye_player_scriptables = Shuffle(eye_scriptables)[0..num_each_item];
        nose_player_scriptables = Shuffle(nose_scriptables)[0..num_each_item];
        mouth_player_scriptables = Shuffle(mouth_scriptables)[0..num_each_item];
        rand_player_words = ShuffleWords(word_list)[0..num_words];

        //Instantiate all the objects
        for(int i = 0; i < num_each_item; i++){
            //Set eye child values (preinstantiated)
            eye_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().SetPart(eye_player_scriptables[i]);
            eye_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().Setup();

            //Set nose child values (preinstantiated)
            nose_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().SetPart(nose_player_scriptables[i]);
            nose_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().Setup();
        
            //Set mouth child values (preinstantiated)
            mouth_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().SetPart(mouth_player_scriptables[i]);
            mouth_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().Setup();
        }

        //Instantiate the words
        for(int i = 0; i < num_words; i++){
            ClickableWord newWord = Instantiate(word_part_prefab, wordBank);
            var randomWord = rand_player_words[i];
            newWord.Setup(wordBank, wordResponseArea, randomWord);
        }
        ShowEyes();
    }

    public void ShowEyes(){
        eye_parent_t.gameObject.SetActive(true);
        eyesDropPoint.enabled = true;
        nose_parent_t.gameObject.SetActive(false);
        noseDropPoint.enabled = false;
        mouth_parent_t.gameObject.SetActive(false);
        mouthDropPoint.enabled = false;
    }
    public void ShowNoses(){
        eye_parent_t.gameObject.SetActive(false);
        eyesDropPoint.enabled = false;
        nose_parent_t.gameObject.SetActive(true);
        noseDropPoint.enabled = true;
        mouth_parent_t.gameObject.SetActive(false);
        mouthDropPoint.enabled = false;
    }
    public void ShowMouths(){
        eye_parent_t.gameObject.SetActive(false);
        eyesDropPoint.enabled = false;
        nose_parent_t.gameObject.SetActive(false);
        noseDropPoint.enabled = false;
        mouth_parent_t.gameObject.SetActive(true);
        mouthDropPoint.enabled = true;
    }

    public Sprite GetSpriteFromPartName(string partName)
    {
        Debug.Log(partName);
        if((partName != null && partName != "Empty") && partsDictionary.TryGetValue(partName, out var part))
        {
            Debug.Log("Found Part!");
            return part.GetImage();
        }
        else
        {
            return null;
        }
    }
}
