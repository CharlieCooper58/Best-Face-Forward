using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] int num_each_item;
    [SerializeField] int num_words;
    [SerializeField] private Part[] eye_scriptables;
    [SerializeField] private Part[] nose_scriptables;
    [SerializeField] private Part[] mouth_scriptables;
    [SerializeField] private string[] word_list;

    private Part[] eye_player_scriptables;
    private Part[] nose_player_scriptables;
    private Part[] mouth_player_scriptables;
    private string[] rand_player_words;
    [SerializeField] private GameObject img_part_prefab;
    [SerializeField] private GameObject word_part_prefab;

    [SerializeField] private Transform eye_parent_t;
    [SerializeField] private Transform nose_parent_t;
    [SerializeField] private Transform mouth_parent_t;
    [SerializeField] private Transform word_parent_t;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < num_each_item; i++){
            Instantiate(img_part_prefab, eye_parent_t);
            Instantiate(img_part_prefab, nose_parent_t);
            Instantiate(img_part_prefab, mouth_parent_t);
            Instantiate(word_part_prefab, word_parent_t);
        }
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
            int k = Random.Range(0,n+1);
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
            int k = Random.Range(0,n+1);
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
            Part word = new Part();
            word.SetType(FeatureType.word);
            word.SetWord(rand_player_words[i]);
            word_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().SetPart(mouth_player_scriptables[i]);
            word_parent_t.GetChild(i).GetComponentInChildren<DraggableImage>().Setup();
        }
    }
}
