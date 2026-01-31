using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Scriptable Objects/Part")]
public class Part : ScriptableObject
{
    public string ID;
    public Sprite partImage;

    public enum FeatureType
    {
        eyes,
        nose,
        mouth
    }
    public FeatureType type;
}
