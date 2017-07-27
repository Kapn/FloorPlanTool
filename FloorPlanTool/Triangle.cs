using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    class Triangle : IShape
    {   
        public Triangle() { LineWidth = 2; }
        public Triangle(Point Location, PointF[] Points, Color LineColor)
        {
            this.Location = Location;
            this.Points = Points;
            this.LineColor = LineColor;
            LineWidth = 2;
        }

        public Point Location { get; set; }
        public PointF[] Points { get; set; }
        public PointF Point1 { get; set; }
        public PointF Point2 { get; set; }
        public PointF Point3 { get; set; }
        public int Size { get; set; }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddPolygon(Points);            
            return path;
        }

        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
            using (var pen = new Pen(LineColor, LineWidth))                
                result = path.IsVisible(p);
            return result;
        }

        public void Draw(Graphics g)
        {
            using (var path = GetPath())
            using (var pen = new Pen(LineColor, LineWidth))
            {                                                
                g.DrawPolygon(pen, Points);
            }
        }

        public void Move(Point d)
        {            
            Points[0] = new PointF(Points[0].X + d.X, Points[0].Y + d.Y);
            Points[1] = new PointF(Points[1].X + d.X, Points[1].Y + d.Y);
            Points[2] = new PointF(Points[2].X + d.X, Points[2].Y + d.Y);
            Location = new Point((int)(Points[2].X + d.X), (int)(Points[2].Y + d.Y));

        }

        public void Resize(Point e, Point distance)
        {          
            var scale = e.Y - distance.Y;            

            Points[0] = new PointF((float)(Location.X + scale * Math.Cos(Math.PI / 3)),
                              (float)(Location.Y + scale * Math.Sin(Math.PI / 3)));
            Points[1] = new PointF((float)(Location.X + scale * Math.Cos((2 * Math.PI) / 3)),
                                 (float)(Location.Y + scale * Math.Sin((2 * Math.PI) / 3)));
            Points[2] = Location;                    

        }
        
        public List<int> GetProperties()
        {
            return new List<int> { Location.X, Location.Y };
        }

        public IShape Copy()
        {            
            return new Triangle(Location, Points, LineColor);
        }
    }
}
