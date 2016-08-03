using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

using TGame = Game;

public class Client {

    static public Client inst;


    public Dictionary<int, ActorClient> actors;
    public Dictionary<int, GameObject> points;

    public TGame game;
    public float time;
    public float dt;

    public float worldSize;

    public ActorClient self;
    public FollowObj camerFollow;

    public Client()
    {
        inst = this;
        //actors = new Dictionary<int, ActorClient>();
        //points = new Dictionary<int, GameObject>();
    }

    public void SetCamera(float r)
    {
        if (camerFollow!=null)
        {
            camerFollow.SetXValue(r * 2);
        }
    }

    public void Update(float value)
    {
        dt = value;
        time = time + dt;

        foreach (var a in actors)
        {
            a.Value.Update();
        }
    }


    // event ------------
    public void PlayerMove(Vector2 forward)
    {
        game.PlayerMove(forward.x,forward.y);
    }

    public void EnterGame(TGame g)
    {
        game = g;

        ReStart();
    }

    public void ReStart()
    {   
        if (actors!=null)
        {
            foreach(var a in actors)
            {
                GameObject.Destroy(a.Value.obj);
            }
        }

        if (actors != null)
        {
            foreach (var a in points)
            {
                GameObject.Destroy(a.Value);
            }
        }
        actors = new Dictionary<int, ActorClient>();
        points = new Dictionary<int, GameObject>();

        game.Enter(this);       
    }

    // cmd 

    public void CmdEnter(float time, PlayerInfo info,float size,
        PlayerInfo[] infos,
        ValuePoint[] ps)
    {        
        game.callId = info.id;

        ActorClient a = actors[info.id];

        self = a;
        
        camerFollow = Camera.main.GetComponent<FollowObj>();
        camerFollow.obj = a.obj;

        worldSize = size;

        for (int i=0; i< infos.Length; i++)
        {
            CmdActorAppear(infos[i]);
        }

        for (int i = 0; i < ps.Length; i++)
        {
            CmdAddPoint(ps[i]);
        }
    }

    public void CmdActorAppear(PlayerInfo info)
    {        
        if (actors.ContainsKey(info.id) == false)
        {
            var a = new ActorClient(info);
            actors.Add(info.id, a);
        }
        
    }

    public void CmdMove(float time, int id,float x,float y,float sx,float sy,float dx,float dy)
    {
        var a = actors[id];
        if (a != null)
        {    
            a.CmdMove(time, x, y, sx, sy, dx, dy);                  
        }
    }

    public void CmdAddValue(int id, float value)
    {
        var a = actors[id];
        if (a != null)
        {
            a.AddRadius(value);
        }
    }


    public void CmdDelActor( int id)
    {
        var a = actors[id];
        if (a != null)
        {
            actors.Remove(id);
            a.OnDeath();
        }
    }


    public void CmdAddPoint(ValuePoint p)
    {
        int index = UnityEngine.Random.Range(1, 3);
        GameObject obj = Tools.LoadGameObject("Point/Pt" + index);
        obj.transform.localPosition = new Vector3(p.x, 0, p.y);
        points.Add(p.id, obj);
    }

    public void CmdDelPoint(int id)
    {
        if (points.ContainsKey(id))
        {
            GameObject.Destroy(points[id]);
            points.Remove(id);
        }       

    }
}
