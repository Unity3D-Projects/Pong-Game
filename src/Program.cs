namespace Pong {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Windows.Forms;

using PrimusGE.Core;
using PrimusGE.Graphics.SharpDXImpl;
using PrimusGE.Sound.SharpDXImpl;

using Scenes;

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
