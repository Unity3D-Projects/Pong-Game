namespace PongBrain.Base.Sound.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Diagnostics;

using SharpDX.Multimedia;
using SharpDX.XAudio2;


/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXSound: IDisposable, ISound {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private AudioBuffer m_Buffer;

    private uint[] m_PacketsInfo;

    private WaveFormat m_Format;

    private SharpDXSoundMgr m_Sound;

    private Stopwatch m_Stopwatch = new Stopwatch();

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int MaxPlaysPerSec { get; set; } = 10;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXSound(SharpDXSoundMgr sound, AudioBuffer buffer, WaveFormat format, uint[] packetsInfo) {
        m_Buffer = buffer;
        m_Format = format;
        m_Sound  = sound;
        m_PacketsInfo = packetsInfo;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Play(float pitch) {
        if (MaxPlaysPerSec > 0) {
            var minTime = 1.0f / MaxPlaysPerSec;
            var time    = (float)m_Stopwatch.Elapsed.TotalSeconds;

            if (m_Stopwatch.IsRunning && time < minTime) {
                return;
            }
        }

        var sourceVoice = m_Sound.GetVoice();

        sourceVoice.SubmitSourceBuffer(m_Buffer, m_PacketsInfo);
        sourceVoice.Start();

        m_Stopwatch.Restart();
    }

    public void Stop() {
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            m_Sound = null;
        }
    }
}

}
