namespace PongBrain.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Entity {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly Dictionary<Type, object> m_Components;

    private static int s_NextID = 1;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int ID { get; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Entity() {
        m_Components = new Dictionary<Type, object>();

        ID = Interlocked.Increment(ref s_NextID);
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void AddComponent(object component) {
        m_Components.Add(component.GetType(),  component);
    }

    public void AddComponents(params object[] components) {
        foreach (var component in components) {
            AddComponent(component);
        }
    }

    public T GetComponent<T>() {
        return (T)GetComponent(typeof (T));
    }

    public object GetComponent(Type type) {
        object component;
        m_Components.TryGetValue(type, out component);
        return component;
    }

    public object[] GetComponents() {
        return new List<object>(m_Components.Values).ToArray();
    }

    public bool HasComponent<T>() {
        return HasComponent(typeof (T));
    }

    public bool HasComponent(Type type) {
        return m_Components.ContainsKey(type);
    }

    public bool RemoveComponent<T>() {
        return RemoveComponent(typeof (T));
    }

    public bool RemoveComponent(Type type) {
        return m_Components.Remove(type);
    }
}

}
