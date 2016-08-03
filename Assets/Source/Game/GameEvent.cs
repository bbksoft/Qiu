using UnityEngine;
using System.Collections;

using TClient = Client;

public class GameEvent {

	static public void Call(string fun,params object[] objs)
    {
        if (Client.inst == null)
        {
            return;
        }

        var t = typeof(Client);
        var method = t.GetMethod(fun);
        try
        {
            method.Invoke(Client.inst, objs);
        }
        catch
        {
            Debug.Log("call error:");
            Debug.Log(fun);
        }
    }

    static public void ServerCall(TClient inst,string fun, params object[] objs)
    {
        if (inst == null) return;

        var t = typeof(TClient);
        var method = t.GetMethod("Cmd" + fun);
        try
        {
            method.Invoke(inst, objs);
        }
        catch
        {
            Debug.Log("server call error:");
            Debug.Log(fun);
        }
    }
}
