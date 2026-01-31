using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    Part[] parts;
    Dictionary<string, Part> partsDict;
    void Start()
    {
        partsDict = new Dictionary<string, Part>();
        for(int i = 0; i<parts.Length; i++)
        {
            partsDict.Add(parts[i].name, parts[i]);
        }
    }
}
