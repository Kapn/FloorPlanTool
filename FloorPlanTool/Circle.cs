using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    // ---------------------------------
    //  Handles Drawing/Manipulating Circles
    // ---------------------------------
    public class Circle : IShape
    {
        public Circle() {
            FillColor = Color.Black;
            Fill = false;
        }

        public Circle(int Radius, Point Center, bool Fill, Color FillColor)
        {
            this.FillColor = FillColor;
            this.Fill = Fill;
            this.Radius = Radius;
            this.Center = Center;
        }
        
        public bool Fill { get; set; }
        public Color FillColor { get; set; }
        public Point Center { get; set; }
        private int radius;
        public int Radius
        {
            get { return radius; }
            set { radius = value; }
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
            if (Fill)
            {
                using (var path = GetPath())
                using (var brush = new SolidBrush(FillColor))
                    g.FillPath(brush, path);
            } else
            {
                var p = Center;
                p.Offset(-Radius, -Radius);

                using (var path = GetPath())
                using (var pen = new Pen(FillColor))
                    g.DrawEllipse(pen, new Rectangle(p, new Size(Radius * 2, 2* Radius)));
            }
            
        }
        public void Move(Point d)
        {
            Center = new Point(Center.X + d.X, Center.Y + d.Y);
        }

        public void Resize(Point e, Point previousPoint)
        {
            int newRadius;
            int dx = e.X - previousPoint.X;
            int dy = e.Y - previousPoint.Y;

            if (dx < dy)
            {
                newRadius = dy;
            }
            else
            {
                newRadius = dx;
            }

            this.Radius = newRadius;
        }

        public List<int> GetProperties()
        {
            if (Fill)
            {                
                return new List<int> { Radius, Center.X, Center.Y, 1 };
            } else
            {
                return new List<int> { Radius, Center.X, Center.Y, 0 };
            }            
        }

        public IShape Copy()
        {            
            return new Circle(Radius, Center, Fill, FillColor);            
        }

        string IShape.ToString()
        {
            string shape_info = String.Format("Circle\nRadius: {0} , Center: ({1}, {2})", Radius, Center.X, Center.Y);
            return shape_info;
        }
    }
}
