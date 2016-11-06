namespace PongBrain.Base.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GameForm: Form {
    public GameForm() {
        FormClosed += (sender, e) => Game.Inst.Exit();

        DoubleBuffered  = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;

        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
                 true);
    }
}

}
