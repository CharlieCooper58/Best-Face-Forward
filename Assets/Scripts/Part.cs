using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Scriptable Objects/Part")]
public class Part : ScriptableObject
{
    [SerializeField] private string ID;
    [SerializeField] private Sprite partImage;
    [SerializeField] private string word;
    public FeatureType type;
    public string GetID(){
        return ID;
    }
    public Sprite GetImage(){
        return partImage;
    }
    public FeatureType GetPartType(){
        return type;
    }
    public string GetWord(){
        return word;
    }
}
public enum FeatureType
    {
        eyes,
        nose,
        mouth,
        word
    }
