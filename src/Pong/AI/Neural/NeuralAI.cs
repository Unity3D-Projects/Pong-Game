namespace Pong.AI.Neural {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class NeuralAI {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private Entity m_Ball;

    private Entity m_Paddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public NeuralAI(Entity paddle, Entity ball) {
        m_Ball   = ball;
        m_Paddle = paddle;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Think(float dt) {
    }
}

}
