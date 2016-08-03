using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIClick : MonoBehaviour,
    IPointerUpHandler
{
    public string fun;

    public void OnPointerUp(PointerEventData data)
    {        
        GameEvent.Call(fun);
    }
}
