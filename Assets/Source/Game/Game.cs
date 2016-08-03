

using System;
using System.Collections.Generic;


using TClient = Client;

public struct PlayerInfo
{
    public int id;
    public float x;
    public float y;

    public float radius;
    public float xAcc;
    public float yAcc;
    public float xSpeed;
    public float ySpeed;


    public PlayerInfo(int v_id, float v_x, float v_y)
    {
        id = v_id;
        x = v_x;
        y = v_y;

        xAcc = 0;
        yAcc = 0;

        xSpeed = 0;
        ySpeed = 0;

        radius = 0.5f;
    }

    public PlayerInfo(Actor a)
    {
        id = a.id;
        x = a.x.value;
        y = a.y.value;

        xAcc = a.x.acc;
        yAcc = a.y.acc;

        xSpeed = a.x.speed;
        ySpeed = a.y.speed;

        radius = a.radius;
    }
}
public struct ValuePoint
{
    const float PT_RADIUS = 0.2f;

    public int id;
    public float x;
    public float y;

    public bool IsColl(Actor a)
    {
        float dx = a.x.value - x;
        float dy = a.y.value - y;
        //float dz = a.radius - PT_RADIUS;


        float dis2 = dx * dx + dy * dy;// + dz * dz;
        //float r = PT_RADIUS + a.radius;
        //if (dis2 <= r*r)
        if (dis2 <= a.radius * a.radius)
        {
            return true;
        }

        return false;
    }

    public ValuePoint(int vId, float vX, float vY)
    {
        id = vId;
        x = vX;
        y = vY;
    }

}


public class Game {

    //public const float DT = 0.1f;
    public static float DT = 0.1f;

    public const float PointValue = 0.2f;

    static public Game inst;


    // client -> server messag call 
    public int callId;

    public int maxId;

    public int robotCount;

    public Dictionary<int,Actor> actors;
    public Dictionary<int,ValuePoint> points;

    public AreaManager areas;
    public float worldSize;

    public float time;
    public float dt;

    public Game()
    {
        inst = this;
        maxId = 0;
        actors = new Dictionary<int, Actor>();
        points = new Dictionary<int,ValuePoint>();
        areas = new AreaManager();
        areas.Init(10, 5);
        time   = 0;
        robotCount = 0;
        worldSize = areas.GetWorldSize();
    }

    public void Update()
    {
        time = time + DT;

        List<int> ids = new List<int>();
 
        foreach (var a in actors)
        {
            if (a.Value.radius > 0)
            {
                a.Value.Update(ids);
            }
        }

        foreach (var id in ids)
        {
            areas.DelActor(id);
        }

        ids.Clear();
        foreach (var p in points)
        {
            bool needDel = false;
            foreach (var a in actors)
            {
                Actor actor = a.Value;
                if ( p.Value.IsColl(actor) )
                {
                    needDel = true;

                    actor.AddValue(PointValue);

                    break;
                }
            }    
            
            if (needDel)
            {
                ids.Add(p.Key);                
            }             
        }

        foreach(var id in ids)
        {
            areas.DelPoint(id);
        }

        areas.Update();
    }

    public PlayerInfo[] GetAllActorInfo()
    {
        PlayerInfo[] ret = new PlayerInfo[actors.Count];

        int n = 0;
        foreach(var a in actors)
        {
            ret[n] = new PlayerInfo(a.Value);
            n++;
        }

        return ret;
    }

    public ValuePoint[] GetAllPointInfo()
    {
        ValuePoint[] ret = new ValuePoint[points.Count];

        int n = 0;
        foreach (var a in points)
        {
            ret[n] = a.Value;
            n++;
        }        

        return ret;
    }

    public void Enter(TClient c)
    {
        PlayerInfo ret = areas.CreateActor(c);
        PlayerInfo[] actors = GetAllActorInfo();
        ValuePoint[] pts = GetAllPointInfo();
        GameEvent.ServerCall(c, "Enter", time, ret, worldSize,
            actors, pts);
    }

    public void PlayerMove(float x, float y)
    {
        if (actors.ContainsKey(callId))
        { 
            Actor a = actors[callId];
            a.Move(x, y);
        }            
    }

    public void BoardCast(float x, float y, string fun, params object[] objs)
    {
        foreach (var a in actors)
        {
            GameEvent.ServerCall(a.Value.client,fun, objs);
        }
    }

    Random r = new Random(10);
    public float rand(float max)
    {
        return (float)r.NextDouble() * (max-0.0001f);
    }

}
