using UnityEngine;

[CreateAssetMenu(fileName = "ThemeBank", menuName = "Themes/ThemeBank")]
public class ThemeBank : ScriptableObject
{
    [System.Serializable]
    public struct ThemeClassPair
    {
        public string theme;
        public string prompt;
    }
    public ThemeClassPair[] pairs;

    public ThemeClassPair GetRandomTheme()
    {
        return pairs[Random.Range(0, pairs.Length)];
    }
}
