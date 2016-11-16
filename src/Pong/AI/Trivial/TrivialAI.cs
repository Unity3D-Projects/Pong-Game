namespace Pong.AI.Trivial {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class TrivialAI {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private Entity m_Ball;

    private Entity m_Paddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public TrivialAI(Entity paddle, Entity ball) {
        m_Ball   = ball;
        m_Paddle = paddle;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Think() {
        var ballPos   = m_Ball.GetComponent<BodyComponent>().Position;
        var paddlePos = m_Paddle.GetComponent<BodyComponent>().Position;
        var controls  = m_Paddle.GetComponent<ControlsComponent>().Controls;

        var d = ballPos.Y - paddlePos.Y;
        var r = 10.0f*Math.Abs(d);
        var y = Math.Min(r, 1.0f) * Math.Sign(d);

        controls["Y"] = y;

    }
}

}
