namespace PongBrain.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Game {
    /*-------------------------------------
     * PRIVATE CONSTANTS
     *-----------------------------------*/

    private const double INV_UPDATES_PER_SEC = 1.0 / 120.0;
    private const double INV_DRAWS_PER_SEC = 1.0 / 60.0;

    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private bool m_Done;

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
        m_Window = CreateForm(title, width, height);
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

            Application.DoEvents();
        }

        LeaveScene();
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private Form CreateForm(string title, int width, int height) {
        var form = new GameForm();

        form.FormClosed += (sender, e) => Exit();

        form.Size = new Size(width, height);
        form.Text = title;

        form.Show();

        return form;
    }
}

}
