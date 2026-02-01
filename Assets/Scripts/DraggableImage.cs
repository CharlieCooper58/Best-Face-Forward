using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableImage : DraggableItem
{
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //Setup objects
        Setup();
    }
    public void Setup(){
        image = GetComponent<Image>();
        image.sprite = my_part.GetImage();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(dragging){
            transform.position = Mouse.current.position.value;
        }
        //This draggable object was released
        if(Mouse.current.leftButton.wasReleasedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            if(!UnderMouse(mouse_check)){
                return;
            }
            Transform new_parent = GetDropParent(mouse_check);
            if(new_parent != null){
                bool can_add = false;
                //This draggable object is over home or a correct drop type
                if(new_parent.CompareTag("home")){
                    if(new_parent.gameObject.GetComponentInChildren<DraggableItem>()==null){
                        //Remove the part from the manager
                        print("Removing the image from the face and placing it home");
                        ResponseManager.rM.RemoveItem(my_part);
                        can_add = true;
                    }
                } else {
                    //We know it matches the part so we're adding a part to the response
                    print("Adding a facial feature to the response.");
                    can_add = ResponseManager.rM.AddItem(my_part);
                }
                //If there is no conflicting thing in the slot
                if(can_add){
                    transform.parent = new_parent;
                    transform.position = transform.parent.position;
                }
            } else {
                transform.position = transform.parent.position;
            }
        }
    }
    public Transform GetDropParent(List<RaycastResult> ui_under_mouse){
        //Checks if the mouse is over a place where this item can be dropped
        if(ui_under_mouse == null){
            return null;
        }
        foreach(RaycastResult r in ui_under_mouse){
            if(r.gameObject.CompareTag("home") && r.gameObject.GetComponentInChildren<DraggableItem>()==null){
                return r.gameObject.transform;
            }
            if(r.gameObject.GetComponent<DraggableSlot>() != null){
                if(r.gameObject.GetComponent<DraggableSlot>().GetSlotType() == my_part.GetPartType()){
                    return r.gameObject.transform;
                }
            }
        }
        return null;
    }
}
