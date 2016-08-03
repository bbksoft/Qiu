

using System;
using System.Collections.Generic;

using TClient = Client;

public class Area  {
    public Dictionary<int, Actor> actors;
    public Dictionary<int, ValuePoint> points;

    public float time;

    public Area()
    {
        actors = new Dictionary<int, Actor>();
        points = new Dictionary<int, ValuePoint>();
        time = 0;
    }
}

public class AreaManager
{
    const int MAX_POINT_COUNT = 10;

    public Area[,] areas;
    public float unitSize;
    public float unitCount;
    public int maxPTId;

    public float GetWorldSize()
    {
        return unitSize * unitCount;
    }

    public void Init(float size,int count)
    {
        unitSize = size;
        unitCount = count;
        areas = new Area[count,count];
        maxPTId = 0;

        for (int i=0; i< count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                areas[i, j] = new Area();

                for (int k = 0; k < MAX_POINT_COUNT/2; k++)
                {
                    RandomPoint(i, j);
                }
            }
        }
    }

    public void Update()
    {
        for (int i = 0; i < unitCount; i++)
        {
            for (int j = 0; j < unitCount; j++)
            {
                if (areas[i, j].points.Count < MAX_POINT_COUNT)
                {
                    areas[i, j].time -= Game.DT;

                    if (areas[i, j].time <= 0)
                    {
                        areas[i, j].time = 1;
                        RandomPoint(i, j);
                    }
                }
            }
        }
    }

    public void RandomPoint(int i, int j)
    {
        maxPTId++;
        float x = Game.inst.rand(unitSize) + i * unitSize;
        float y = Game.inst.rand(unitSize) + j * unitSize;

        ValuePoint p = new ValuePoint(maxPTId, x, y);

        areas[i, j].points.Add(maxPTId, p);
        Game.inst.points.Add(maxPTId,p);

        Game.inst.BoardCast(x, y, "AddPoint", p);
    }

    public void DelPoint(int id)
    {
        ValuePoint a = Game.inst.points[id];
        Area area = GetArea(a);
        if (area != null)
        {
            area.points.Remove(id);
        }
        Game.inst.points.Remove(id);

        Game.inst.BoardCast(a.x, a.y, "DelPoint", id);
    }

    public PlayerInfo CreateActor(TClient c)
    {
        Game.inst.maxId++;

        float x = Game.inst.rand(Game.inst.worldSize);
        float y = Game.inst.rand(Game.inst.worldSize);

        PlayerInfo ret = new PlayerInfo(Game.inst.maxId, x, y);

        Actor a = new Actor(ret, c);
        AddActor(a);
        

        return ret;
    }

    public void CreateRobot()
    {
        Game.inst.maxId++;

        float x = Game.inst.rand(Game.inst.worldSize);
        float y = Game.inst.rand(Game.inst.worldSize);

        PlayerInfo ret = new PlayerInfo(Game.inst.maxId, x, y);

        Robot a = new Robot(ret);
        AddActor(a);    
    }

    void AddActor(Actor a)
    {
        Area area = GetArea(a);
        
        if (area != null)
        {
            area.actors.Add(a.id,a);
        }
        Game.inst.actors.Add(a.id, a);

        PlayerInfo ret = new PlayerInfo(a);
        Game.inst.BoardCast(a.x.value, a.y.value,"ActorAppear", ret);
    }

    public void DelActor(int id)
    {
        if (Game.inst.actors.ContainsKey(id) == false )
        {
            return;
        }

        Actor a = Game.inst.actors[id];

        Game.inst.BoardCast(a.x.value, a.y.value, "DelActor", a.id);

        Area area = GetArea(a);
        if (area!=null)
        {
            area.actors.Remove(id);
        }
        Game.inst.actors.Remove(id);       
    }

    public Area GetArea(Actor a)
    {
        int x = (int)(a.x.value / unitSize);
        int y = (int)(a.y.value / unitSize);
        
        return areas[x,y];
    }

    public Area GetArea(ValuePoint a)
    {
        int x = (int)(a.x / unitSize);
        int y = (int)(a.y / unitSize);

        return areas[x, y];
    }
}

