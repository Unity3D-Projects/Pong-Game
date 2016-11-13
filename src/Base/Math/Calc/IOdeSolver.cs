namespace PongBrain.Base.Math.Calc {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IOdeSolver {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Solve(IOde eq, float dt, Action<float[], float[]> derivsFn);
}

}
