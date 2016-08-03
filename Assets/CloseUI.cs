using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CloseUI : MonoBehaviour,
    IPointerUpHandler
{

    public void OnPointerUp(PointerEventData data)
    {
        Transform t = transform;

        while ( t.parent.parent != null )
        {
            t = t.parent;
        }

        GameObject.Destroy(t.gameObject);
    }
}
