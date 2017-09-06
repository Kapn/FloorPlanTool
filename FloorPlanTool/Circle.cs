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
    // Properties:
    //      Fill : Bool determines whether the shape should be filled
    //      FillColor : Fill Color
    //      Center :  Point at the center of the circle
    //      Radius : Radius of circle    
    // Methods:
    //      GetPath()       : Returns GraphicsPath object used to perform HitTest
    //      HitTest(Point p): Checks if the point is within the object's Path
    //      Draw(Graphics g): Draws the object
    //      Move(Point d)   : Moves
    //      Resize(Point e, Point previousPoint) : Resize 
    //      Copy(): Returns a Copy of the object
    //      ToString(): Returns a string of all properties
    [Serializable]
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
        public int Radius { get; set; }

        public override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            var p = Center;
            p.Offset(-Radius, -Radius);
            path.AddEllipse(p.X, p.Y, 2 * Radius, 2 * Radius);
            return path;
        }

        public override bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
                result = path.IsVisible(p);
            return result;
        }
        public override void Draw(Graphics g)
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
                
                using (var pen = new Pen(FillColor))
                    g.DrawEllipse(pen, new Rectangle(p, new Size(Radius * 2, 2* Radius)));
            }
            
        }
        public override void Move(Point d)
        {
            Center = new Point(Center.X + d.X, Center.Y + d.Y);
        }

        public override void Resize(Point e, Point previousPoint)
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

        public override IShape Copy()
        {            
            return new Circle(Radius, Center, Fill, FillColor);            
        }

        public override string ToString()
        {
            return String.Format("Circle\nRadius: {0} , Center: ({1}, {2})", Radius, Center.X, Center.Y);            
        }
    }
}
