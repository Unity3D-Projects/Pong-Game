namespace PongBrain.AI {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Input;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class SimplePaddleAI {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Entity m_Ball;
    private Entity m_Paddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SimplePaddleAI(Entity ball, Entity paddle) {
        m_Ball   = ball;
        m_Paddle = paddle;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Think(float dt) {
        var ballPos   = m_Ball.GetComponent<PositionComponent>();
        var paddlePos = m_Paddle.GetComponent<PositionComponent>();
        var paddleVel = m_Paddle.GetComponent<VelocityComponent>();

        var controls = m_Paddle.GetComponent<ControlsComponent>().Controls;

        controls["Y"] = 0.0f;

        if (ballPos.Y < paddlePos.Y) {
            controls["Y"] = -1.0f;
        }
        else {
            controls["Y"] = 1.0f;
        }
    }
}

}
