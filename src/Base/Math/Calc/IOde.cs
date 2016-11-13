namespace PongBrain.Base.Math.Calc {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IOde {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    float[] Derivs { get; }
    float[] PrevState { get; }
    float[] State { get; }
}

}
