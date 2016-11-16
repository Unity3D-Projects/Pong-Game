namespace Pong.Base.Math.Calc {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IIntegrator {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Solve(IOde eq, float dt, Action<float[], float[]> derivsFn);
}

}
