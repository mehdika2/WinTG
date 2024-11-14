using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTG.Engine
{
    public class Scene
    {
        public Scene(string filename, string name, List<Node> nodes)
        {
            this.filename = filename;
            this.name = name;
            this.nodes = nodes;
        }

        internal string filename { get; set; }
        public string name { get; set; }
        public List<Node> nodes { get; private set; }
    }
}
