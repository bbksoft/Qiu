#define USED_ACC

using System;
using System.Collections.Generic;
using TClient = Client;

public class XSpeed
{
    const float Deceleration    = 5.0f;
    const float Acceleration    = 10.0f;
    const float MaxSpeed        = 3.0f;

    public float value;
    public float speed;
    public float acc;

    public void Update(float dt,float radius, float size)
    {
        float power = 1 / (float)Math.Sqrt(radius);

#if USED_ACC
        float a = Acceleration * acc * dt * power;

        if ( speed > 0)
        {
            a -= Deceleration * dt * power;
            speed += a;
            if (speed > MaxSpeed* power)
            {
                speed = MaxSpeed * power;
            }
        }
        else if (speed < 0)
        {
            a += Deceleration * dt * power;
            speed += a;
            if (speed < -MaxSpeed * power)
            {
                speed = -MaxSpeed * power;
            }
        }
        else
        {
            speed += a;
        }    

        value += speed * dt;  
#else
        value += acc * MaxSpeed * dt;
#endif

        if (value < 0)
        {
            value = 0;
        }
        else if ( value >= size )
        {
            value = size - 0.0001f;
        }
    }
};

public class Actor {
  

    public TClient client;

    public int id;
    public XSpeed x;
    public XSpeed y;

    public float radius;

    public Actor(PlayerInfo info, TClient c)
    {
        client = c;


        x = new XSpeed();
        y = new XSpeed();

        id = info.id;

        x.value = info.x;
        y.value = info.y;

        y.speed = 0;
        y.speed = 0;

        x.acc = 0;
        y.acc = 0;

        radius = info.radius;
    }

    public void Move(float vx, float vy)
    {
        float s = (float)Math.Sqrt(vx * vx + vy * vy);

        if ( s > 0)
        {
            x.acc = vx / s;
            y.acc = vy / s;
        }
        else
        {
            x.acc = 0;
            y.acc = 0;
        }

        Game.inst.BoardCast(x.value, y.value,
            "Move", Game.inst.time, id,
            x.value, y.value, x.speed, y.speed, x.acc, y.acc);
    }

    virtual public void Update(List<int> ids)
    {
        x.Update(Game.DT, radius, Game.inst.worldSize);
        y.Update(Game.DT, radius, Game.inst.worldSize);
 
        foreach (var a in Game.inst.actors)
        {
            Actor actor = a.Value;
            if ( (actor.radius>0) && (radius >  actor.radius * 1.2f) )
            {
                float dx = actor.x.value - x.value;
                float dy = actor.y.value - y.value;

                float dis2 = dx * dx + dy * dy;
                float r = actor.radius + radius;

                if (dis2 < r * 2)
                {
                    AddValue(actor.radius);
                    actor.radius = 0;
                    ids.Add(a.Key);
                }
            }
        }
    }

    public void AddValue(float r)
    {
        //float value = radius * radius * radius + r * r * r;
        //value = (float)Mathf.Pow(value,1.0f / 3) - radius;

        float value = radius * radius + r * r;
        value = (float)Math.Sqrt(value) - radius;

        radius += value;
        Game.inst.BoardCast(x.value, y.value, "AddValue", id, value);
    }
}
