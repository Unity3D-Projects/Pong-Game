namespace PongBrain.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class Scene {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly Dictionary<Type, List<Entity>> m_ComponentTypeToEntityMap;
    private readonly Dictionary<int, Entity> m_Entities;
    private readonly List<Entity> m_EntitiesToAdd;
    private readonly HashSet<int> m_EntitiesToRemove;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public IEnumerable<Entity> Entities {
        get { return m_Entities.Values; }
    }

    public Scene Parent { get; internal set; }

    public IReadOnlyList<Subsystem> Subsystems { get; private set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Scene() {
        m_ComponentTypeToEntityMap = new Dictionary<Type, List<Entity>>();
        m_Entities                 = new Dictionary<int, Entity>();
        m_EntitiesToAdd            = new List<Entity>();
        m_EntitiesToRemove         = new HashSet<int>();
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public virtual void AddEntity(Entity entity) {
        m_EntitiesToAdd.Add(entity);
    }

    public virtual void AddEntities(params Entity[] entities) {
        foreach (var entity in entities) {
            AddEntity(entity);
        }
    }

    public virtual void Cleanup() {
        foreach (var subsystem in Subsystems) {
            subsystem.Cleanup();
        }
    }

    public virtual void Draw(float dt) {
        foreach (var subsystem in Subsystems) {
            subsystem.Draw(dt);
        }
    }

    public virtual IEnumerable<Entity> GetEntities<T>() {
        List<Entity> entities;
        if (!m_ComponentTypeToEntityMap.TryGetValue(typeof (T), out entities)) {
            return new Entity[0];
        }

        return entities;
    }

    public virtual void Init() {
        foreach (var subsystem in Subsystems) {
            subsystem.Init();
        }
    }

    public virtual bool RemoveEntity(int id) {
        if (!m_Entities.ContainsKey(id)) {
            return false;
        }

        m_EntitiesToRemove.Add(id);
        return true;
    }

    public void SetSubsystems(params Subsystem[] subsystems) {
        Contract.Assert(Subsystems == null);

        Subsystems = new List<Subsystem>(subsystems).AsReadOnly();
    }

    public virtual void Update(float dt) {
        foreach (var entity in m_EntitiesToAdd) {
            AddEntityInternal(entity);
        }

        m_EntitiesToAdd.Clear();

        foreach (var subsystem in Subsystems) {
            subsystem.Update(dt);
        }

        foreach (var id in m_EntitiesToRemove) {
            RemoveEntityInternal(id);
        }

        m_EntitiesToRemove.Clear();
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void AddEntityInternal(Entity entity) {
        m_Entities.Add(entity.ID, entity);

        foreach (var component in entity.GetComponents()) {
            var type = component.GetType();

            List<Entity> components;

            if (!m_ComponentTypeToEntityMap.TryGetValue(type, out components)) {
                components = new List<Entity>();
                m_ComponentTypeToEntityMap[type] = components;
            }

            components.Add(entity);
        }
    }

    private void RemoveEntityInternal(int id) {
        Entity entity;

        if (!m_Entities.TryGetValue(id, out entity)) {
            return;
        }

        foreach (var component in entity.GetComponents()) {
            var type     = component.GetType();
            var entities = m_ComponentTypeToEntityMap[type];

            entities.Remove(entity);
        }
    }
}

}
