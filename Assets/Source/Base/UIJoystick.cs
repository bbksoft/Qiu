using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public Image back;
    public Image point;
    public float range = 50;
    public string fun;

    Vector2 start;  

    void Start()
    {
        back.enabled = false;
        point.enabled = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        Vector2 pos = GetUIPos(data);

        back.enabled = true;
        point.enabled = true;

        start = pos;

        back.transform.localPosition = pos;
        point.transform.localPosition = pos;
    }

    public void OnPointerUp(PointerEventData data)
    {
        back.enabled = false;
        point.enabled = false;

        GameEvent.Call(fun, Vector2.zero);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 pos = GetUIPos(data);
        Vector2 v = pos - start;

        float dis = v.magnitude;
             
        if ( dis > range)
        {
            point.transform.localPosition = start + range * v.normalized;
        }
        else
        {
            point.transform.localPosition = pos;
        }

        GameEvent.Call(fun, v);     
    }

    Vector2 GetUIPos(PointerEventData data)
    {
        return data.position - new Vector2(Screen.width / 2, Screen.height / 2);
    }   
}
