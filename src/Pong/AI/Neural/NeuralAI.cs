namespace PongBrain.Pong.AI.Neural {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class NeuralAI {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Entity m_Ball;

    private Entity m_Paddle;

    private NeuralNetwork m_Brain;

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
