namespace PongBrain.AI.Neural {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class NeuralPaddleAI {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Entity m_Ball;

    private Entity m_Paddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public NeuralPaddleAI(Entity paddle, Entity ball) {
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
