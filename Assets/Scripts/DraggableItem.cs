using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] private Part my_part;
    private bool dragging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragging = false;
        //Setup objects
        if(my_part.GetPartType()==FeatureType.word){
            GetComponentInChildren<TextMeshProUGUI>().text = my_part.GetID();
        } else {
            GetComponent<Image>().sprite = my_part.GetImage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging){
            transform.position = Mouse.current.position.value;
        }
        if(Mouse.current.leftButton.wasPressedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            //Technically if you move too fast while still pressing down
            //the item can stop being under and then you have to go pick it up again.
            if(UnderMouse(mouse_check)){ 
                dragging = true;
            }
        } else if(Mouse.current.leftButton.wasReleasedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            if(UnderMouse(mouse_check)){
                dragging = false;
                //This draggable object was released
                if(OverDrop(mouse_check)){
                    //This draggable object is over home or a correct drop type
                    Transform new_parent = GetDropParent(mouse_check).transform;
                    bool can_add = false;
                    //Check that the response doesn't already have a feature of that type
                    can_add = ResponseManager.rM.AddItem(my_part);
                    //However, if the new parent is back home
                    if (new_parent.CompareTag("home")){
                        //Makes sure that there isn't anything else on that tile
                        if(new_parent.GetComponentInChildren<DraggableItem>() == null){
                            ResponseManager.rM.RemoveItem(my_part);
                            can_add = true;
                        }
                    }
                    //Add the object if the response doesn't have that feature
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
        print(ui_under_mouse);
        if(ui_under_mouse == null){
            return false;
        }
        foreach(RaycastResult r in ui_under_mouse){
            print(r.gameObject.name);
            //Try to place it home if it's home
            if(r.gameObject.CompareTag("home")){
                return true;
            }
            //Otherwise check to see if it's in the right type of slot
            if(r.gameObject.GetComponent<DraggableSlot>() != null){
                if(r.gameObject.GetComponent<DraggableSlot>().GetSlotType() == my_part.GetPartType()){
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
            if(r.gameObject.CompareTag("home")){
                return r.gameObject;
            }
            if(r.gameObject.GetComponent<DraggableSlot>() != null){
                if(r.gameObject.GetComponent<DraggableSlot>().GetSlotType() == my_part.GetPartType()){
                    return r.gameObject;
                }
            }
        }
        return null;
    }
}
