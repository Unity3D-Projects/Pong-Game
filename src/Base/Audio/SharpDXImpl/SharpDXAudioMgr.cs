namespace PongBrain.Base.Audio {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Core;

using SharpDX.DirectSound;
using SharpDX.Multimedia;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXAudioMgr: IAudioMgr {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private DirectSound m_DirectSound;

    private PrimarySoundBuffer m_PrimaryBuffer;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXAudioMgr() {

    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
        if (m_PrimaryBuffer != null) {
            m_PrimaryBuffer.Dispose();
            m_PrimaryBuffer = null;
        }

        if (m_DirectSound != null) {
            m_DirectSound.Dispose();
            m_DirectSound = null;
        }
    }

    public void Init() {
        m_DirectSound = new DirectSound();

        m_DirectSound.SetCooperativeLevel(Game.Inst.Window.Handle,
                                          CooperativeLevel.Priority);

        var bufDesc = new SoundBufferDescription {
            AlgorithmFor3D = Guid.Empty,
            Flags          = BufferFlags.PrimaryBuffer
        };

        m_PrimaryBuffer = new PrimarySoundBuffer(m_DirectSound, bufDesc);
        m_PrimaryBuffer.Play(0, PlayFlags.Looping);
        
    }

    public object Load(string path) {
        var bufDesc = new SoundBufferDescription {
            AlgorithmFor3D = Guid.Empty,
            Flags          = BufferFlags.ControlPositionNotify | BufferFlags.GetCurrentPosition2
        };

        var buf = new SecondarySoundBuffer(m_DirectSound, bufDesc);
        return null;
    }
}

}
