namespace PongBrain.Messaging {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class MessageInfo {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public object Data { get; }

    public string Message { get; }


    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public MessageInfo(string message, object data) {
        Data    = data;
        Message = message;
    }
}

}
