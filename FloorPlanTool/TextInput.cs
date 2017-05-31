using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    public class TextInput : IShape
    {
        public TextInput() {
                             FontSize = 12;
                             Font = new Font("Arial", FontSize);
                             Brush = new SolidBrush(Color.Black); }
        public int FontSize { get; set; }
        public SolidBrush Brush { get; set; }
        public Font Font { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        private Rectangle drawRect;

        public void Draw(Graphics g)
        {
            using (var path = GetPath())            
                g.DrawString(Text, Font, Brush, drawRect);
        }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            drawRect = new Rectangle(PosX, PosY, Width, Height);
            path.AddRectangle(drawRect);
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
            PosX = PosX + d.X;
            PosY = PosY + d.Y;
        }

        public void Resize(Point e, Point previousPoint)
        {
            int dx = e.X - PosX;
            int dy = e.Y - PosY;

            Console.WriteLine("dx:" + dx);
            Width = dx;
            Height = dy;

            //FontSize = CurrentFontSize * (DesiredWidth / CurrentWidth)

            FontSize = dx/6;
            Console.WriteLine("fontsize:" + FontSize);
            Font = new Font("Arial", FontSize);


        }
    }
}
