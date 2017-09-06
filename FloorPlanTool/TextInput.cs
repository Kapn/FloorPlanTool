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
    // Properties:
    //      FontSize : Font Size
    //      Font : Font object, takes Font Size as parameter
    //      TextColor :  Text Color
    //      Text : string of text to be drawn
    //      PosX : Top Left point's x-position
    //      PosY: Top Left point's y-position
    //      Width: Width of rectangle the Text fits in
    //      Height: Height of rectangle the Text fits in
    // Methods:
    //      GetPath()       : Returns GraphicsPath object used to perform HitTest
    //      HitTest(Point p): Checks if the point is within the object's Path
    //      Draw(Graphics g): Draws the object
    //      Move(Point d)   : Moves
    //      Resize(Point e, Point previousPoint) : Resize 
    //      Copy(): Returns a Copy of the object
    //      ToString(): Returns a string of all properties
    [Serializable]
    public class TextInput : IShape
    {
        #region Constructors
        public TextInput()
        {
            // Set a default Font, FontSize, and Brush
            FontSize = 12;
            Font = new Font("Arial", FontSize);
            TextColor = Color.Black;
        }

        public TextInput(int PosX, int PosY, string Text, Color TextColor, int FontSize, Font Font)
        {
            this.FontSize = FontSize;
            this.Font = Font;
            this.TextColor = TextColor;
            this.PosX = PosX;
            this.PosY = PosY;
            this.Text = Text;
        }
        #endregion

        public int FontSize { get; set; }
        public Color TextColor { get; set; }
        public Font Font { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        private Rectangle drawRect;

        public override void Draw(Graphics g)
        {            
            using (var brush = new SolidBrush(TextColor))
            {
                // Strip rectangle to only be size of Text.
                // This is done by adjusting Width and Height based off of the input string.
                var stringSize = g.MeasureString(Text, Font);
                Width = Convert.ToInt32(stringSize.Width) + 5;
                //Height = Convert.ToInt32(stringSize.Height);

                GetPath(); //call GetPath to set up 'drawRect' w/ new Width
                g.DrawString(Text, Font, brush, drawRect);
            }         
                
        }

        public override GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            drawRect = new Rectangle(PosX, PosY, Width, Height);
            path.AddRectangle(drawRect);
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
            PosX = PosX + d.X;
            PosY = PosY + d.Y;
        }

        public override void Resize(Point e, Point previousPoint)
        {
                        
            Width = e.X - PosX;
            Height = e.Y - PosY;
            
            //TODO: this is a random scaling to determine fontSize
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

        public override IShape Copy()
        {            
            return new TextInput(PosX, PosY, Text, TextColor, FontSize, Font);            
        }

        public override string ToString()
        {
            return String.Format("TextInput\nLocation: ({0}, {1}),  Width: {2}, Height: {3}, String: {4}", PosX, PosY, Width, Height, Text);            
        }
    }
}
