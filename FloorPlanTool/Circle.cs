using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    public class Circle : IShape
    {
        public Circle() { FillColor = Color.Black; }
        public Color FillColor { get; set; }
        public Point Center { get; set; }
        private int radius;
        public int Radius
        {
            get { return radius; }
            set { this.radius = value; }
        }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            var p = Center;
            p.Offset(-Radius, -Radius);
            path.AddEllipse(p.X, p.Y, 2 * Radius, 2 * Radius);
            return path;
        }

        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
                result = path.IsVisible(p);
            return result;
        }
        public void Draw(Graphics g)
        {
            using (var path = GetPath())
            using (var brush = new SolidBrush(FillColor))
                g.FillPath(brush, path);
        }
        public void Move(Point d)
        {
            Center = new Point(Center.X + d.X, Center.Y + d.Y);
        }
        public void Resize(int radius)
        {
            this.Radius = radius;
        }
    }
}
