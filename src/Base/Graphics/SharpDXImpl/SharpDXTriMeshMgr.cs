namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Math;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTriMeshMgr: IDisposable, ITriMeshMgr {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private SharpDXGraphics m_Graphics;

    private List<SharpDXTriMesh> m_TriMeshes = new List<SharpDXTriMesh>();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTriMeshMgr(SharpDXGraphics graphics) {
        m_Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public ITriMesh CreateQuad(float width, float height) {
        var hh = 0.5f*height;
        var hw = 0.5f*width;

        var verts = new [] {
            new Vertex { Position=new Vector4(-hw, -hh), TexCoord=new Vector2(0.0f, 1.0f) },
            new Vertex { Position=new Vector4(-hw,  hh), TexCoord=new Vector2(0.0f, 0.0f) },
            new Vertex { Position=new Vector4( hw, -hh), TexCoord=new Vector2(1.0f, 1.0f) },
            new Vertex { Position=new Vector4( hw,  hh), TexCoord=new Vector2(1.0f, 0.0f) },
        };

        var tris = new [] {
            0, 1, 2,
            0, 2, 3
        };

        var vertexBuffer = D3D11.Buffer.Create(m_Graphics.Device,
                                               D3D11.BindFlags.VertexBuffer,
                                               verts);

        var triMesh = new SharpDXTriMesh(m_Graphics, vertexBuffer, 4, 2);

        m_TriMeshes.Add(triMesh);

        return triMesh;
    }

    public void Dispose() {
        foreach (var triMesh in m_TriMeshes) {
            triMesh.Dispose();
        }

        m_TriMeshes.Clear();
        m_TriMeshes = null;

        m_Graphics = null;
    }
}

}
