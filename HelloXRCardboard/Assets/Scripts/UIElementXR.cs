using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementXR : MonoBehaviour
{
    public UnityEvent onXRPointerEnter;
    public UnityEvent onXRPointerExit;
    private Camera cam;

    void Start()
    {
        this.cam = CameraPointerManager.instance.gameObject.GetComponent<Camera>();
    }

    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = this.PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerEnterXR() {
        GazeManager.Instance.SetUpGaze(1.5f);
        this.onXRPointerEnter?.Invoke();
        PointerEventData pointerEvent = this.PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerExitXR() {
        GazeManager.Instance.SetUpGaze(2.5f);
        this.onXRPointerEnter?.Invoke();
        PointerEventData pointerEvent = this.PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerUpHandler);
    }

    public PointerEventData PlacePointer() {
        var screenPos = this.cam.WorldToScreenPoint(CameraPointerManager.instance.hitPoint);
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(screenPos.x, screenPos.y);
        return pointer;
    }

    
}
