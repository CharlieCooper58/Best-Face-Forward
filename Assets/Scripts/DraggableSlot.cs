using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.InputSystem.XR;

public class DraggableSlot : MonoBehaviour
{
    [SerializeField] private FeatureType slot_type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public FeatureType GetSlotType(){
        return slot_type;
    }
}
