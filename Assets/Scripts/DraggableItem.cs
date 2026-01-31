using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] private string[] drop_name;
    [SerializeField] private Part my_part;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.isPressed){
            List<RaycastResult> mouse_check = IsOverUI();
            //Technically if you move too fast while still pressing down
            //the item can stop being under and then you have to go pick it up again.
            if(UnderMouse(mouse_check)){ 
                transform.position = Mouse.current.position.value;
            }
        } else if(Mouse.current.leftButton.wasReleasedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            if(UnderMouse(mouse_check)){
                //This draggable object was released
                if(OverDrop(mouse_check)){
                    Transform new_parent = GetDropParent(mouse_check).transform;
                    bool can_add = false;
                    if(new_parent.CompareTag("merge")){
                        can_add = ResponseManager.rM.AddItem(my_part);
                    } else if (new_parent.CompareTag("home")){
                        ResponseManager.rM.RemoveItem(my_part);
                        can_add = true;
                    }
                    if(can_add){
                        transform.parent = new_parent;
                    }
                }
                if(transform.position != transform.parent.position) {
                    //Eventually run a coroutine / timer that linearly interpolates to goal.
                    transform.position = transform.parent.position;
                }
            }
        }
    }
    public List<RaycastResult> IsOverUI(/*string tag*/){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.value;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        /*for(int i = 0; i < raycastResults.Count; i++){
            if(!raycastResults[i].gameObject.CompareTag(tag)){
                raycastResults.RemoveAt(i);
                i--;
            }
        }*/
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
    public bool OverDrop(List<RaycastResult> ui_under_mouse){
        //Checks if the mouse is over a place where this item can be dropped
        if(ui_under_mouse == null){
            return false;
        }
        foreach(RaycastResult r in ui_under_mouse){
            foreach(string s in drop_name){
                if(r.gameObject.CompareTag(s)){
                    return true;
                }
            }
        }
        return false;
    }
    public GameObject GetDropParent(List<RaycastResult> ui_under_mouse){
        //Checks if the mouse is over a place where this item can be dropped
        if(ui_under_mouse == null){
            return null;
        }
        foreach(RaycastResult r in ui_under_mouse){
            foreach(string s in drop_name){
                if(r.gameObject.CompareTag(s)){
                    return r.gameObject;
                }
            }
        }
        return null;
    }
}
