namespace PongBrain {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Windows.Forms;

using AI.Neural;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal static class Program {
    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    [STAThread]
    private static void Main(string[] args) {
        Application.EnableVisualStyles();

        var game = Game.Inst;

        game.Init("PongBrain", 640, 480);
        game.Run(new Scenes.MainScene());
    }
}

}
