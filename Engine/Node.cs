using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTG.Engine
{
    public class Node
    {
        Dictionary<string, object> properties;
        internal Guid id { get; }
        public string name { get; set; }
        public Vector pos { get; set; }
        public object tag { get; set; }
        internal NLua.Lua scriptEngine { get; set; }
        internal string scriptText { get; set; }
        private string _script;
        public string script
        {
            get
            {
                return _script;
            }
            internal set
            {
                if (value.StartsWith("~"))
                {
                    scriptText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, value.Remove(0, 1)));
                }
                else
                {
                    scriptText = File.ReadAllText(value);
                }
                _script = value;
                scriptEngine = new NLua.Lua();
                scriptEngine.DoString(_script);
            }
        }

        public Node(string name, int x, int y)
        {
            properties = new Dictionary<string, object>();
            id = Guid.NewGuid();
            this.name = name;
            this.pos = new Vector(x, y);
        }

        public object this[string propertyName]
        {
            get
            {
                if (properties.ContainsKey(propertyName))
                {
                    return properties[propertyName];
                }
                else
                {
                    throw new ArgumentException($"Property '{propertyName}' is not defined");
                }
            }
            set
            {
                properties[propertyName] = value;
            }
        }

        public override string ToString()
        {
            return name + ", " + pos.x + ", " + pos.y;
        }
    }
}
