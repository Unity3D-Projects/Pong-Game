namespace PongBrain.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Components;
using Messaging;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Game {
    /*-------------------------------------
     * PRIVATE CONSTANTS
     *-----------------------------------*/

    private const double INV_DRAWS_PER_SEC = 1.0 / 60.0;
    private const double INV_UPDATES_PER_SEC = 1.0 / 120.0;

    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private bool m_Done;

    private readonly Dictionary<Type, List<Action<IMessage>>> m_MessageHandlers;

    private readonly Queue<IMessage> m_MessageQueue;

    private Scene m_Scene;

    private Form m_Window;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static Game Inst { get; } = new Game();

    public Scene Scene {
        get { return m_Scene; }
    }

    public Form Window {
        get { return m_Window; }
    }
    
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    private Game() {
        m_MessageHandlers = new Dictionary<Type, List<Action<IMessage>>>();
        m_MessageQueue    = new Queue<IMessage>();
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void AddEntity(Entity entity) {
        m_Scene.AddEntity(entity);
    }

    public void EnterScene(Scene scene) {
        scene.Parent = m_Scene;
        scene.Init();

        m_Scene = scene;
    }

    public void Exit() {
        m_Done = true;
    }

    public IEnumerable<Entity> GetEntities() {
        return m_Scene.Entities;
    }

    public IEnumerable<Entity> GetEntities<T>() {
        return m_Scene.GetEntities<T>();
    }

    public void Init(string title, int width, int height) {        
        m_Window = CreateWindow(title, width, height);
    }

    public void LeaveScene() {
        if (m_Scene == null) {
            return;
        }

        m_Scene.Cleanup();
        m_Scene = m_Scene.Parent;

        if (m_Scene == null) {
            Exit();
        }
    }

    public void OnMessage<T>(Action<IMessage> cb) where T: IMessage {
        OnMessage(typeof (T), cb);
    }

    public void OnMessage(Type type, Action<IMessage> cb) {
        List<Action<IMessage>> callbacks;
        if (!m_MessageHandlers.TryGetValue(type, out callbacks)) {
            callbacks = new List<Action<IMessage>>();
            m_MessageHandlers[type] = callbacks;
        }

        callbacks.Add(cb);
    }

    public void PostMessage(IMessage message) {
        m_MessageQueue.Enqueue(message);
    }

    public bool RemoveEntity(int id) {
        return m_Scene.RemoveEntity(id);
    }

    public void Run(Scene scene) {
        m_Done = false;

        EnterScene(scene);

        var t1 = 0.0;
        var t2 = 0.0;
        var stopwatch = Stopwatch.StartNew();
        while (!m_Done) {
            var dt = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            t1 += dt;
            t2 += dt;

            while (t1 >= INV_UPDATES_PER_SEC) {
                m_Scene.Update((float)INV_UPDATES_PER_SEC);
                t1 -= INV_UPDATES_PER_SEC;
            }

            while (t2 >= INV_DRAWS_PER_SEC) {
                m_Scene.Draw((float)INV_DRAWS_PER_SEC);
                t2 -= INV_DRAWS_PER_SEC;
            }

            DispatchMessages();

            Application.DoEvents();
        }

        LeaveScene();
    }

    public Entity SetTimeout(Action cb, float time) {
        var timer = new Entity();

        timer.AddComponent(new LifetimeComponent { EndOfLife = cb,
                                                   Lifetime  = time });

        AddEntity(timer);
        return timer;
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private Form CreateWindow(string title, int width, int height) {
        var form = new GameForm();

        form.FormClosed += (sender, e) => Exit();

        form.Size = new Size(width, height);
        form.Text = title;

        form.Show();

        return form;
    }

    private void DispatchMessages() {
        while (m_MessageQueue.Count > 0) {
            var msg = m_MessageQueue.Dequeue();

            List<Action<IMessage>> callbacks;
            if (!m_MessageHandlers.TryGetValue(msg.GetType(), out callbacks)) {
                continue;
            }

            foreach (var cb in callbacks) {
                cb(msg);
            }
        }
    }
}

}
