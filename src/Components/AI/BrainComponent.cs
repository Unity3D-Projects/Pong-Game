namespace PongBrain.Components.AI {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BrainComponent {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float ThinkTimer;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Action<float> ThinkFunc { get; set; }

    public float ThinkRate { get; set; } = 30.0f;
}

}
