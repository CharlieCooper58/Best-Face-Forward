using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableImage : DraggableItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //Setup objects
        GetComponent<Image>().sprite = my_part.GetImage();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //This draggable object was released
        if(Mouse.current.leftButton.wasReleasedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            Transform new_parent = GetDropParent(mouse_check);
            if(new_parent != null){
                //This draggable object is over home or a correct drop type
                if(new_parent.CompareTag("home")){
                    //Remove the part from the manager
                    print("Removing the image from the face and placing it home");
                } else {
                    //We know it matches the part so we're adding a part to the response
                    print("Adding a facial feature to the response.");
                }
                transform.parent = new_parent;
            }
        }
        if(dragging){
            print("Dragging");
            transform.position = Mouse.current.position.value;
        }
        if(transform.position != transform.parent.position) {
            //Eventually run a coroutine / timer that linearly interpolates to goal.
            transform.position = transform.parent.position;
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
