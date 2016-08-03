
using UnityEngine;


public class ActorClient {

    public int id;
    public XSpeed x;
    public XSpeed y;

    public float radius;

    public Vector2 disPos;

    public GameObject obj;

    public ActorClient(PlayerInfo info)
    {
        x = new XSpeed();
        y = new XSpeed();

        x.value = info.x;
        y.value = info.y;

        x.acc = info.xAcc;
        y.acc = info.yAcc;

        x.speed = info.xSpeed;
        y.speed = info.ySpeed;

        radius = info.radius;

        disPos.x = x.value;
        disPos.y = y.value;

        obj = Tools.LoadGameObject("Actor/Ball");

        obj.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        obj.transform.localPosition = new Vector3(
             disPos.x, radius, disPos.y);
    }

    public void Update()
    {
        x.Update(Client.inst.dt, radius, Client.inst.worldSize);
        y.Update(Client.inst.dt, radius, Client.inst.worldSize);

        disPos = Vector2.Lerp(disPos, new Vector2(x.value, y.value), 0.3f);

        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale,
                new Vector3(radius * 2, radius * 2, radius * 2), 0.3f);                
        obj.transform.localPosition = new Vector3(
           disPos.x, obj.transform.localScale.y / 2, disPos.y);
    }

    public void CmdMove(float time, float _x, float _y, float sx, float sy, float dx, float dy)
    {

        x.value = _x;
        y.value = _y;

        x.acc = dx;
        y.acc = dy;

        x.speed = sx;
        y.speed = sy;
    }

    public void AddRadius(float r)
    {
        radius += r;

        if ( Client.inst.self == this )
        {
            Client.inst.SetCamera(radius);
        }

        //obj.transform.localScale = new Vector3(radius*2, radius*2, radius*2);
        //obj.transform.localPosition = new Vector3(
        //    obj.transform.localPosition.x, radius, obj.transform.localPosition.z);
    }

    public void OnDeath()
    {
        GameObject.Destroy(obj);

        if (Client.inst.self == this)
        {
            Tools.ShowUI("Lost");
        }
    }

}
