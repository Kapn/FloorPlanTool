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
    //  Handles Drawing/Manipulating Rec
    // ---------------------------------
    // Properties:
    //      Fill : Bool to determine whether the circle should be filled or not
    //      FillColor : Fill Color
    //      Left :  Upper Left point
    //      Right : Bottom Right point
    //      Top   : Upper Left Point
    //      Bottom : Bottom right point
    //      Rec     : Rectangle Object
    //      LineWidth : Line Width
    //      LineColor: Line Color
    // Methods:
    //      GetPath()       : Returns GraphicsPath object used to perform HitTest
    //      HitTest(Point p): Checks if the point is within the object's Path
    //      Draw(Graphics g): Draws the object
    //      Move(Point d)   : Moves
    //      Resize(Point e, Point previousPoint) : Resize 
    //      Copy(): Returns a Copy of the object
    //      ToString(): Returns a string of all properties
    [Serializable]
    class Rec : IShape
    {
        public Rec() {
            FillColor = Color.Black;
            Fill = false;        
        }

        public Rec(int Left, int Top, int Right, int Bottom, bool Fill, Color FillColor)
        {
            this.FillColor = FillColor;
            this.Fill = Fill;
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }
        public bool Fill { get; set; }
        public Color FillColor { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        private Rectangle rec;

        public override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();            
            rec = Rectangle.FromLTRB(Left, Top, Right, Bottom);
            path.AddRectangle(rec);
            return path;
        }

        public override bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
                result = path.IsVisible(p);
            return result;
        }

        public override void Move(Point d)
        {
            Left = Left + d.X;
            Right = Right + d.X;

            Top = Top + d.Y;
            Bottom = Bottom + d.Y;

        }

        /*
         * Draw Rec object.  If Fill, then we FillPath
         *                   Else DrawRectangle based off local var 'rec'
         */
        public override void Draw(Graphics g)
        {
            if (Fill)
            {
                using (var path = GetPath())
                using (var brush = new SolidBrush(FillColor))
                    g.FillPath(brush, path);
            } else
            {
                //call GetPath() to setup 'rec' variable
                using (var path = GetPath()) 
                using (var pen = new Pen(FillColor))
                    g.DrawRectangle(pen, rec);
            }
        }

        //resize rectangle
        //top-left corner should stay fixed
        public override void Resize(Point e, Point previousPoint)
        {
            int dx = e.X - Right;
            int dy = e.Y - Bottom;
                        
            Right = Right + dx;
            Bottom = Bottom + dy;

            // Handle mirroring across Y-Axis
            if (Right < previousPoint.X)
            {
                var initialRight = Right;
                Right = previousPoint.X;
                Left = initialRight;                
            }
            
            // Handle mirroring across X-Axis
            if (Bottom < previousPoint.Y)
            {
                var initialBottom = Bottom;
                Bottom = previousPoint.Y;
                Top = initialBottom;                
            }
        }
        
        public override IShape Copy()
        {            
            return new Rec(Left, Top, Right, Bottom, Fill, FillColor);            
        }

        public override string ToString()
        {
            return String.Format("Rectangle\nLeft: {0}, Right: {1}, Top: {2}, Bottom: {3}", Left, Right, Top, Bottom);            
        }
    }
}
