using UnityEngine;
using System.Collections;

public class TestGame : MonoBehaviour {

    Game g;
    Client c;

    float dt;

    // Use this for initialization
    void Start () {
        g = new Game();
        c = new Client();
        c.EnterGame(g);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        c.Update(Time.fixedDeltaTime);

        dt += Time.fixedDeltaTime;        
        while (dt >= Game.DT)
        {
            dt -= Game.DT;
            g.Update();
        }

        if ( g.robotCount < 10 )
        {
            g.areas.CreateRobot();
        }
	}
}
