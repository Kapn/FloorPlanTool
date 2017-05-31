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
        public Rec() {
            FillColor = Color.Black;
            Fill = false;        
        }
        public bool Fill { get; set; }
        public Color FillColor { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        private Rectangle temp_rec;
        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();            
            temp_rec = Rectangle.FromLTRB(Left, Top, Right, Bottom);
            path.AddRectangle(temp_rec);
            return path;
        }

        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
                result = path.IsVisible(p);
            return result;
        }

        public void Move(Point d)
        {
            Left = Left + d.X;
            Right = Right + d.X;

            Top = Top + d.Y;
            Bottom = Bottom + d.Y;

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
                using (var path = GetPath())
                using (var pen = new Pen(FillColor))
                    g.DrawRectangle(pen, temp_rec);
            }


        }

        //resize rectangle
        //top-left corner stays fixed
        public void Resize(Point e, Point previousPoint)
        {
            int dx = e.X - Right;
            int dy = e.Y - Bottom;
            
            Right = Right + dx;
            Bottom = Bottom + dy;
            
        }

    }
}
