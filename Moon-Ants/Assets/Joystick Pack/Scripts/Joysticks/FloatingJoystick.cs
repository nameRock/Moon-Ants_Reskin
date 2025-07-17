using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) == false && Input.touchCount == 0 && eventData.pointerDrag != null)
        {
            eventData.pointerDrag = null;
            background.gameObject.SetActive(false);
            return;
        }
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void OnApplicationFocus(bool focus)
    {
        background.gameObject.SetActive(false);
        base.OnApplicationFocus(focus);
    }

    protected override void OnEnable()
    {
        background.gameObject.SetActive(false);
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        background.gameObject.SetActive(false);
        base.OnDisable();
    }
}