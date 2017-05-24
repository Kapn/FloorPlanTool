using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    class Rec : IShape
    {
        public Rec() { FillColor = Color.Black; }
        public Color FillColor { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        private Rectangle temp_rec;
        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();            
            temp_rec = Rectangle.FromLTRB(Left, Right, Top, Bottom);
            path.AddRectangle(temp_rec);
            return path;
        }

        public bool HitTest(Point p)
        {
            throw new NotImplementedException();
        }

        public void Move(Point d)
        {
            throw new NotImplementedException();
        }

        public void Draw(Graphics g)
        {
            using (var path = GetPath())
            using (var pen = new Pen(FillColor))
                g.DrawRectangle(pen, temp_rec);
        }

        public void Resize(int radius)
        {
            throw new NotImplementedException();
        }

    }
}
