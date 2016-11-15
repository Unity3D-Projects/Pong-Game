namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTriMesh: IDisposable, ITriMesh {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public D3D11.VertexBufferBinding Binding;

    public SharpDXGraphicsMgr Graphics;

    public D3D11.Buffer VertexBuffer;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int NumTris { get; private set; }

    public int NumVerts { get; private set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTriMesh(SharpDXGraphicsMgr graphics,
                         D3D11.Buffer     vertexBuffer,
                         int              numVerts,
                         int              numTris)
    {
        var size = Marshal.SizeOf<Vertex>();

        Binding      = new D3D11.VertexBufferBinding(vertexBuffer, size, 0);
        Graphics     = graphics;
        VertexBuffer = vertexBuffer;

        NumTris  = numTris;
        NumVerts = numVerts;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (VertexBuffer != null) {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }

            Binding  = default (D3D11.VertexBufferBinding);
            Graphics = null;
        }
    }
}

}
