namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Input;
using Core;
using Input;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class InputSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Draw(float dt) {
        base.Draw(dt);

        var keyboard = new Keyboard();

        foreach (var entity in Game.Inst.GetEntities<InputComponent>()) {
            var input = entity.GetComponent<InputComponent>();

            input.ResetControls?.Invoke();

            foreach (var e in input.KeyMap) {
                if (!keyboard.IsKeyPressed(e.Key)) {
                    continue;
                }

                e.Value();
            }
        }
    }
}

}
