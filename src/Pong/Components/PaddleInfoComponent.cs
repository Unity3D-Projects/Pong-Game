namespace PongBrain.Pong.Components {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class PaddleInfoComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Speed { get; set; } = 8.0f;

    public float TiltAngle { get; set; } = 0.14f*(float)Math.PI;
}

}
