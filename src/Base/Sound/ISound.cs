namespace PongBrain.Base.Sound {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ISound {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    int MaxPlaysPerSec { get; set; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Play(float pitch=1.0f);

    void Stop();
}

}
