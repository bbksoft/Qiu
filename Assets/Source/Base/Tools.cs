using UnityEngine;
using System.Collections;

public class Tools {

   static public GameObject LoadGameObject(string res)
   {
        var o = LoadRes(res);

        var obj = GameObject.Instantiate(o) as GameObject;

        return obj;
   }

    static public void ShowUI(string res)
    {
        var o = LoadRes("UI/" + res);

        var obj = GameObject.Instantiate(o) as GameObject;

        GameObject canvas =  GameObject.Find("Canvas");

        obj.transform.parent = canvas.transform;

        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;

        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt)
        {
            rt.sizeDelta = new Vector2(0, 0);
        }


    }

    static public Object LoadRes( string res)
    {
        var o = Resources.Load(res);

        if (o == null)
        {
            Debug.LogError("can't found res:" + res);
        }

        return o;
    }
}
