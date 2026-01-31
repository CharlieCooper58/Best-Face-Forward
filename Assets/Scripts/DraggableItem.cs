using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] protected Part my_part;
    protected bool dragging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        dragging = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame){
            print("Mouse clicked");
            List<RaycastResult> mouse_check = IsOverUI();
            if(UnderMouse(mouse_check)){ 
                dragging = true;
            }
        } else if(Mouse.current.leftButton.wasReleasedThisFrame){
            print("Mouse released");
            List<RaycastResult> mouse_check = IsOverUI();
            if(UnderMouse(mouse_check)){
                dragging = false;
            }
        }
    }
    public List<RaycastResult> IsOverUI(){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.value;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if(raycastResults.Count > 0){
            return raycastResults;
        } else {
            return null;
        }
    }
    public bool UnderMouse(List<RaycastResult> ui_under_mouse){
        //Checks if this object is in the list of objects under the mouse
        if(ui_under_mouse == null){
            return false;
        }
        foreach(RaycastResult r in ui_under_mouse){
            if(r.gameObject == this.gameObject){
                return true;
            }
        }
        return false;
    }
}
