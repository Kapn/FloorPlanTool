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
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ShapesList() : base()
        {

        }


        public void Save(string file)
        {
            using (Stream stream = File.Open(file, FileMode.Create))
            {
                //BinaryFormatter bin = new BinaryFormatter();
                //bin.Serialize(stream, this);
                Serialize(this, stream);
            }
        }
        public void Load(string file)
        {
            using (Stream stream = File.Open(file, FileMode.Open))
            {
                var shapes = Deserialize(stream);
                //BinaryFormatter bin = new BinaryFormatter();
                //var shapes = (ShapesList)bin.Deserialize(stream);
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

        public void Serialize(Dictionary<int, IShape> dictionary, Stream stream)
        {
            //stream = new MemoryStream();
            //new BinaryFormatter().Serialize(stream, dictionary);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(dictionary.Count);
            foreach (var obj in dictionary)
            {
                writer.Write(obj.Key);
                writer.Write(obj.Value.ToString());
            }
            writer.Flush();
        }

        public Dictionary<int, IShape> Deserialize(Stream stream)
        {
            //return (Dictionary<int, IShape>)new BinaryFormatter().Deserialize(stream);

            BinaryReader reader = new BinaryReader(stream);
            int count = reader.ReadInt32();
            var temp_dictionary = new Dictionary<int, string>(count);
            for (int n = 0; n < count; n++)
            {
                var key = reader.ReadInt32();
                var value = reader.ReadString();
                temp_dictionary.Add(key, value);
            }            
            var dictionary = new Dictionary<int, IShape>();
            foreach (var obj in temp_dictionary)
            {
                //parse string
                char[] delimeters = {'\n', ','};
                string[] words = obj.Value.Split(delimeters);

                if (words[0] == "Circle")
                {
                    Circle newCircle = new Circle();
                    if (Convert.ToBoolean(words[4]))
                    {
                        newCircle.Fill = true;                        
                    }

                    newCircle.FillColor = handleColor(words[5]);
                    newCircle.Radius = Convert.ToInt32(words[1]);
                    newCircle.Center = new Point(Convert.ToInt32(words[2]), Convert.ToInt32(words[3]));

                    dictionary.Add(obj.Key, newCircle);
                                        
                }
            }
            return dictionary;
        }

        Color handleColor(string color)
        {            
            if (color.Contains("Black"))
            {
                return Color.FromName("Black");
            }
            else if (color.Contains("Red"))
            {
                return Color.FromName("Red");
            }
            else if (color.Contains("Green"))
            {
                return Color.FromName("Green");
            }
            else if (color.Contains("Blue"))
            {
                return Color.FromName("Blue");
            }

            // otherwise error
            return Color.FromName("SlateBlue");            
        }
    }
}
