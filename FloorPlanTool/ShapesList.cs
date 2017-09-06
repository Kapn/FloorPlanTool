using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    [Serializable]
    class ShapesList : Dictionary<int, IShape>
    {
        public void Save(string file)
        {
            using (Stream stream = File.Open(file, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, this);
            }
        }
        public void Load(string file)
        {
            using (Stream stream = File.Open(file, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                var shapes = (ShapesList)bin.Deserialize(stream);
                this.Clear();
                foreach (var shap in shapes)
                {
                    this.Add(shap.Key, shap.Value);
                }
                //this.AddRange(shapes);
            }
        }
        public void Draw(Graphics g)
        {
            foreach (KeyValuePair<int, IShape> shape in this)
                shape.Value.Draw(g);
            //this.ForEach(x => x.Value.Draw(g));
        }
    }
}
