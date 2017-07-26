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
    //  Handles Drawing/Manipulating lines
    // ---------------------------------
    public class Line : IShape
    {
        public Line() { LineWidth = 2; LineColor = Color.Black; }
        public Line(Point point1, Point point2, float[] DashPattern)
        {
            this.DashPattern = DashPattern;
            LineWidth = 2;
            LineColor = Color.Black;
            Point1 = point1;
            Point2 = point2;
        }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }
        public float[] DashPattern { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddLine(Point1, Point2);
            return path;
        }
        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
            using (var pen = new Pen(LineColor, LineWidth + 2))
                result = path.IsOutlineVisible(p, pen);
            return result;
        }
        public void Draw(Graphics g)
        {
            using (var path = GetPath())
            using (var pen = new Pen(LineColor, LineWidth))
            {
                pen.DashPattern = DashPattern;
                g.DrawPath(pen, path);
            }
        }
        public void Move(Point d)
        {
            Point1 = new Point(Point1.X + d.X, Point1.Y + d.Y);
            Point2 = new Point(Point2.X + d.X, Point2.Y + d.Y);
        }

        //resize line based on closest point clicked
        public void Resize(Point e, Point previousPoint)
        {            
            
            double distanceToPt1 = ((e.X - Point1.X) * (e.X - Point1.X) + (e.Y - Point1.Y) * (e.Y - Point1.Y));
            double distanceToPt2 = ((e.X - Point2.X) * (e.X - Point2.X) + (e.Y - Point2.Y) * (e.Y - Point2.Y));

            if (distanceToPt1 < distanceToPt2)
            {
                //leave pt2 anchored
                Point1 = new Point(Point1.X + (e.X - Point1.X), Point1.Y + (e.Y - Point1.Y));
            } else
            {
                //leave pt1 anchored
                Point2 = new Point(Point2.X + (e.X - Point2.X), Point2.Y + (e.Y - Point2.Y));
            }
        }

        public List<int> GetProperties()
        {
            return new List<int> { Point1.X, Point1.Y, Point2.X, Point2.Y };
        }

        public IShape Copy()
        {
            var properties = this.GetProperties();
            return new Line(Point1, Point2, DashPattern);
            //return new Line(new Point(properties[0], properties[1]), new Point(properties[2], properties[3]));
        }
    }
}
