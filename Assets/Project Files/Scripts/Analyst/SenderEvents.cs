using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenderEvents : MonoBehaviour
{
    public void SendClick()
    {
        SendEventClick();
    }
    public void SendOffer()
    {
        SendEventOffer();
    }

    protected virtual void SendEventOffer() { }
    protected virtual void SendEventClick() { }
}
