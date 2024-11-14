using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLua;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinTG.Engine;

namespace WinTG
{
    public partial class MainForm : Form
    {
        #region Constans & Variables
        Thread screenThread;

        Lua lua = new Lua();

        Scene[] scenes;

        Scene currentScene { get { return scenes[activeScene]; } }
        int activeScene = 0;

        int deltaTimeStart = 0;
        #endregion

        #region Initialize
        public MainForm()
        {
            // register builtin functions
            lua.RegisterFunction("createNode", this, typeof(MainForm).GetMethod("createNode", new Type[] { typeof(string), typeof(int), typeof(int) }));
            lua.RegisterFunction("createVector", this, typeof(MainForm).GetMethod("createVector", new Type[] { typeof(int), typeof(int) }));
            lua.RegisterFunction("getNode", this, typeof(MainForm).GetMethod("getNode", new Type[] { typeof(string) }));

            // load sceenes
            if(!LoadScenes())
            {
                Close();
                return;
            }

            // initialize form
            InitializeComponent();

            // referesh screen background thread
            screenThread = new Thread(RefereshScreen)
            {
                IsBackground = true
            };
            screenThread.Start();
        }

        bool LoadScenes()
        {
#if DEBUG
            string projectFilename = Path.Combine(Environment.CurrentDirectory, "Project", "project.json");

#else
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 0)
            {
                Console.WriteLine("Enter project file name as argument please.");
                return false;
            }
            string projectFilename = args[0];
#endif

            List<Scene> tempScenes = new List<Scene>();
            JObject jscenes = JObject.Parse(File.ReadAllText(projectFilename));
            foreach (var jscene in jscenes["scenes"])
            {
                var scene = 
                var s = jscene["name"];
                    var scene = new Scene(projectFilename, jscene["name"], )
            }

            return true;
        }
        #endregion

        #region Render
        void RefereshScreen()
        {
            while (true)
                Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Update(e.Graphics);
            base.OnPaint(e);
        }

        void Update(Graphics g)
        {
            // execute script
            int deltaTime = Environment.TickCount - deltaTimeStart;
            foreach (var script in currentScene.nodes.Where(i => !string.IsNullOrWhiteSpace(i.script)).Select(i => i.script))
            {
                lua.DoString(script);
                lua["deltaTime"] = deltaTime;
                lua.GetFunction("update")?.Call(g);
            }

            foreach (var node in currentScene.nodes)
            {
                g.DrawRectangle(new Pen(Brushes.Black, 2), new Rectangle(node.pos.x, node.pos.y, 50, 50));
            }
            deltaTimeStart = Environment.TickCount;
        }
        #endregion

        #region Built-in Functions
        public Node createNode(string name, int x, int y)
        {
            Node node = new Node(name, x, y);
            currentScene.nodes.Add(node);
            return node;
        }

        public Vector createVector(int x, int y)
        {
            return new Vector(x, y);
        }

        public Node getNode(string name)
        {
            return currentScene.nodes.FirstOrDefault(i => i.name == name);
        }
        #endregion
    }
}
