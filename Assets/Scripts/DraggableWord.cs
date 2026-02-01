using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableWord : DraggableItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //Setup word object
        //Setup();
    }
    public void Setup(){
        if(my_part.GetID() != null){
            GetComponentInChildren<TextMeshProUGUI>().text = my_part.GetID();
        } else {
            Debug.LogError("The ID of a word should not usually be none unless creating at start");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //This draggable object was released
        if(dragging){
            transform.position = Mouse.current.position.value;
        } 
        if(Mouse.current.leftButton.wasReleasedThisFrame){
            List<RaycastResult> mouse_check = IsOverUI();
            if(!UnderMouse(mouse_check)){
                return;
            }
            Transform new_parent = GetDropParent(mouse_check);
            if(new_parent == null){
                transform.parent = transform.parent;
            } else{
                //This draggable object is over home or a correct drop type
                /*if(new_parent.CompareTag("bank")){
                    //Remove the word from the manager
                    print("Removing the word and returning to the word bank.");
                } else {
                    //We know it matches the word so we're adding a word to the response
                    print("Adding a word to the response.");
                }
                print(new_parent.name);*/
                transform.parent = new_parent;
                print(ResponseManager.instance.GetResponse());
            }
            /*if(transform.position != transform.parent.position) {
                //Eventually run a coroutine / timer that linearly interpolates to goal.
                transform.parent = transform.parent;
            }*/
        } 
    }
    public Transform GetDropParent(List<RaycastResult> ui_under_mouse){
        //Checks if the mouse is over a place where this item can be dropped
        if(ui_under_mouse == null){
            return null;
        }
        foreach(RaycastResult r in ui_under_mouse){
            if(r.gameObject.CompareTag("bank")){
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
