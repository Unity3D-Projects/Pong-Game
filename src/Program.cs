namespace PongBrain {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Windows.Forms;

using Base.Core;
using Base.Graphics.SharpDXImpl;
using Base.Sound.SharpDXImpl;
using Pong.Scenes;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal static class Program {
    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    [STAThread]
    private static void Main(string[] args) {
        Application.EnableVisualStyles();

        var game = Game.Inst;

        game.Run(new SharpDXGraphicsMgr(),
                 new SharpDXSoundMgr(),
                 "Pong 0.7a",
                 480, 480,
                 new SplashScene());
    }
}

}
