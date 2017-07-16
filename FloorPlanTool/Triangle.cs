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
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Point Point3 { get; set; }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddPolygon(new Point[] { Point1, Point2, Point3 });
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
                g.DrawPolygon(pen, new Point[]{ Point1, Point2, Point3});
            }
        }

        public void Move(Point d)
        {
            Point1 = new Point(Point1.X + d.X, Point1.Y + d.Y);
            Point2 = new Point(Point2.X + d.X, Point2.Y + d.Y);
            Point3 = new Point(Point3.X + d.X, Point3.Y + d.Y);

        }

        public void Resize(Point e, Point previousPoint)
        {

            //Point1 = new Point(Point1.X + (e.X - Point1.X), Point1.Y + (e.Y - Point1.Y));
            //Point2 = new Point(Point2.X + (e.X - Point2.X), Point2.Y + (e.Y - Point2.Y));
            var delta = (e.X - Point1.X);

            //calc slope of line segment between point3 and point1
            var dy = (Point3.Y - Point1.Y)/Convert.ToInt32(Point3.Y + Point1.Y);
            var dx = (Point3.X - Point1.X)/Convert.ToInt32(Point3.X + Point1.X);
            //var slope = dy / dx;
            //Console.WriteLine(slope);
            
            Point1 = new Point(Point1.X + delta, Point1.Y - delta);
            Point2 = new Point(Point2.X + delta, Point2.Y);
        }
    }
}
