﻿namespace PongBrain.Base.Sound {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ISoundMgr {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Cleanup();
    void Init();

    ISound Load(string path);
}

}
