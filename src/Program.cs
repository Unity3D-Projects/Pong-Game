namespace PongBrain {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Windows.Forms;

using Base.Core;
using Base.Graphics.SharpDXImpl;
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

        game.Run(new SharpDXGraphics(),
                 "PongBrain",
                 480, 480,
                 new MainScene());
    }
}

}
