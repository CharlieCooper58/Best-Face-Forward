using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOverUI("draggable") != null && Mouse.current.leftButton.isPressed){
            print(IsOverUI("draggable")[0].gameObject.name);
            transform.position = Mouse.current.position.value;
        } else {
            if(IsOverUI("merge") != null){
                transform.parent = IsOverUI("merge")[0].gameObject.transform;
            }
            if(transform.position != transform.parent.position){
                transform.position = transform.parent.position;
            }
        }
        /*if(EventSystem.current.currentSelectedGameObject == this.gameObject){
            if(Input.GetKey(KeyCode.Mouse0)){
                //Go to the mouse if mouse clicked on this object
                transform.position = Input.mousePosition;
            } else {
                //If it detects a collidable object
                //if()
                //Place on the face and remove from the inventory
                //Otherwise, return it to the original position
            }
        }*/
    }
    public List<RaycastResult> IsOverUI(string tag){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Mouse.current.position.value;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for(int i = 0; i < raycastResults.Count; i++){
            if(!raycastResults[i].gameObject.CompareTag(tag)){
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        if(raycastResults.Count > 0){
            return raycastResults;
        } else {
            return null;
        }
    }
}
