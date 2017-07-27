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
    //  Handles Drawing/Manipulating Text
    // ---------------------------------
    public class TextInput : IShape
    {
        #region Constructors
        public TextInput()
        {
            FontSize = 12;
            Font = new Font("Arial", FontSize);
            Brush = new SolidBrush(Color.Black);
        }

        public TextInput(int PosX, int PosY)
        {
            FontSize = 12;
            Font = new Font("Arial", FontSize);
            Brush = new SolidBrush(Color.Black);

            this.PosX = PosX;
            this.PosY = PosY;            
        }
        #endregion

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
            {
                // Strip rectangle to only be size of Text.
                // This is done by adjusting Width and Height based off of the input string.
                var stringSize = g.MeasureString(Text, Font);
                Width = Convert.ToInt32(stringSize.Width) + 5;
                //Height = Convert.ToInt32(stringSize.Height);
                
                g.DrawString(Text, Font, Brush, drawRect);
            }         
                
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
                        
            Width = e.X - PosX;
            Height = e.Y - PosY;
            
            //TODO: this is a random scaling to determine fontSize, needs work
            FontSize = Width / 6;
            
            if (FontSize < 8)
            {
                FontSize = 8;
            }

            try
            {
                Font = new Font("Arial", FontSize);
            }
            catch (Exception)
            {
                Console.WriteLine("Font Size less than 0");
            }
            
        }

        public List<int> GetProperties()
        {
            return new List<int> { PosX, PosY };
        }

        public IShape Copy()
        {            
            return new TextInput(PosX, PosY);            
        }
    }
}
