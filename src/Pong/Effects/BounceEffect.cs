namespace PongBrain.Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components.Graphical;
using Base.Core;
using Base.Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BounceEffect: Effect {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    private readonly Entity m_Entity;

    private Matrix4x4 m_OriginalTransform = Matrix4x4.Identity();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BounceEffect(Entity entity, float duration=1.0f): base(duration) {
        m_Entity = entity;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Begin() {
        base.Begin();

        var triMesh = m_Entity.GetComponent<TriMeshComponent>();
        if (triMesh != null) {
            m_OriginalTransform = triMesh.Transform;
        }
    }

    public override void Update(float x) {
        base.Update(x);

        var t  = 2.0f * (float)Math.PI * x;
        var sx = 0.1f*(float)Math.Sin(t*1.0f);
        var sy = 0.1f*(float)Math.Sin(t*1.0f);

        var triMesh = m_Entity.GetComponent<TriMeshComponent>();
        if (triMesh != null) {
            var m = triMesh.Transform;
            m.M11 += sx;
            m.M22 += sy;
            triMesh.Transform = m;
        }
    }
}

}
