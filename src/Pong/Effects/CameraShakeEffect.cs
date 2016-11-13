namespace PongBrain.Pong.Effects {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Math;
using Base.Subsystems;

/*-------------------------------------
 * CLASSES
  *-----------------------------------*/

public sealed class CameraShakeEffect: Effect {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    private readonly GraphicsSubsystem m_GraphicsSubsystem;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public CameraShakeEffect(GraphicsSubsystem graphicsSubsystem, float duration=0.3f): base(duration) {
        m_GraphicsSubsystem = graphicsSubsystem;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void End() {
        base.End();
    }

    public override void Update(float x) {
        base.Update(x);

        var dx = 0.02f*(float)Math.Cos(x*27.0f)*(1.0f-x);
        var dy = 0.02f*(float)Math.Sin(x*31.0f)*(1.0f-x);

        m_GraphicsSubsystem.ViewMatrix = Matrix4x4.Translate(dx, dy, 0.0f);
    }
}

}
