namespace PongBrain.Base.IAudio {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ISound {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Play(float pitch=1.0f);

    void Stop();
}

}
