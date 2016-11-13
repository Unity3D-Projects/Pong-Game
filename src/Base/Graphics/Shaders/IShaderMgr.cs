namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShaderMgr {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    IShader LoadPS<T>(string path) where T: struct;

    IShader LoadPS(string path, Type inputType=null);

    IShader LoadVS<T>(string path) where T: struct;

    IShader LoadVS(string path, Type inputType=null);

}

}
