namespace PongBrain.Base.Messages {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class CollisionMessage: IMessage {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Entity Entity1 { get; }

    public Entity Entity2 { get; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public CollisionMessage(Entity entity1=null, Entity entity2=null) {
        Entity1 = entity1;
        Entity2 = entity2;
    }

}

}
