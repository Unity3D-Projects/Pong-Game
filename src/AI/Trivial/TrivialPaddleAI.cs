namespace PongBrain.AI.Trivial {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Components.Input;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class TrivialPaddleAI {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Entity m_Ball;

    private Entity m_Paddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public TrivialPaddleAI(Entity paddle, Entity ball) {
        m_Ball   = ball;
        m_Paddle = paddle;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Think(float dt) {
        var ballPos   = m_Ball.GetComponent<PositionComponent>();
        var paddlePos = m_Paddle.GetComponent<PositionComponent>();
        var controls  = m_Paddle.GetComponent<ControlsComponent>().Controls;

        var d = ballPos.Y - paddlePos.Y;
        var r = 10.0f*Math.Abs(d);
        var y = Math.Min(r, 1.0f) * Math.Sign(d);

        controls["Y"] = y;

    }
}

}
