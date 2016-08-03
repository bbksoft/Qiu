

using System.Collections.Generic;

public class Robot : Actor
{

    float time;

    public Robot(PlayerInfo info) 
        : base(info,null)
    {
        Game.inst.robotCount++;
    }

    ~Robot()
    {
        Game.inst.robotCount--;
    }

    override public void Update(List<int> ids)
    {
        base.Update(ids);

        time += Game.DT;

        if ( time > 1 )
        {
            float x = Game.inst.rand(1) - 0.5f;
            float y = Game.inst.rand(1) - 0.5f;

            base.Move(x, y);

            time = 0;
        }
    }

}
