using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO: resizing rectangle past nothing should mirror across
// - undo and redo needs a little work
//      - undo text removal and adding

    // Following taken from other version when drawing text, might be useful
//g.SmoothingMode = SmoothingMode.AntiAlias;
//g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//g.PixelOffsetMode = PixelOffsetMode.HighQuality;

namespace FloorPlanTool
{
    public partial class Form1 : Form
    {
        /*
         * Variable Declarations 
         */
        bool drawCir, drawLine,drawRec, drawText, drawDotted, eraser, fill, scaleShape;
        string text_to_draw;
        private SolidBrush brush_color;
        private Color prev_color;
        Bitmap bmap;

        // according to https://stackoverflow.com/questions/336387/image-save-throws-a-gdi-exception-because-the-memory-stream-is-closed
        // memorystreams are acceptable to not close, as they will be closed with bitmap is disposed anyway
        Stream ms;

        IShape lastPopped;      
        public List<IShape> Shapes { get; private set; }
        IShape selectedShape;
        bool moving;
        Point previousPoint = Point.Empty;

        public Form1()
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, drawing_panel, new object[] { true });            
            Shapes = new List<IShape>();
            brush_color = new SolidBrush(Color.Black);
            bmap = new Bitmap(drawing_panel.ClientSize.Width, drawing_panel.ClientSize.Height);
        }
       
        /*
         *  OnPaint Event, Fired with calls to drawing_panel.Invalidate()
         */
        private void drawing_panel_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImage(bmap, Point.Empty);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (var shape in Shapes)
                shape.Draw(e.Graphics);
        }

        private void drawing_panel_MouseDown(object sender, MouseEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmap))
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (drawCir)
                    {
                        previousPoint = e.Location;
                        Circle newCircle = new Circle();
                        if (fill)
                        {
                            newCircle.Fill = true;
                        }
                        newCircle.FillColor = brush_color.Color;
                        newCircle.Center = new Point(e.X, e.Y);
                        newCircle.Radius = trackBar1.Value;
                        Shapes.Add(newCircle);
                        drawing_panel.Invalidate();
                    }
                    //line
                    else if(drawLine)
                    {                        
                        previousPoint = e.Location;
                    }
                    //dotted line
                    else if (drawDotted)
                    {
                        previousPoint = e.Location;
                    }
                    //rectangle
                    else if (drawRec)
                    {                        
                        previousPoint = e.Location;
                    }
                    //draw text
                    else if (drawText)
                    {
                        previousPoint = e.Location;
                        TextBox txtbox = new TextBox { Name = "textbox1" };
                        txtbox.KeyDown += textbox_KeyDown;
                        drawing_panel.Controls.Add(txtbox);
                        txtbox.Location = new Point(e.X, e.Y);
                        txtbox.TextAlign = HorizontalAlignment.Center;
                        
                    }
                    //eraser
                    else if (eraser)
                    {
                        for (var i = Shapes.Count - 1; i >= 0; i--)
                        {
                            if (Shapes[i].HitTest(e.Location))
                            {
                                selectedShape = Shapes[i];
                                if (selectedShape != null)
                                {
                                    Shapes.RemoveAt(i);
                                }
                                break;
                            }
                        }                        
                    }                   
                    //selector
                    else
                    {
                        for (var i = Shapes.Count - 1; i >= 0; i--)
                        {
                            if (Shapes[i].HitTest(e.Location))
                            {
                                selectedShape = Shapes[i];
                                break;
                            }
                        }

                        if (selectedShape != null)
                        {
                            moving = true;
                            previousPoint = e.Location;
                        }
                        drawing_panel.Invalidate();
                    }
                }
                // resize with wheel click or right click
                else
                {
                    
                    for (var i = Shapes.Count - 1; i >= 0; i--)
                    {
                        //Console.WriteLine("type:" + Shapes[i].ToString());
                        if (Shapes[i].HitTest(e.Location))
                        {
                            selectedShape = Shapes[i];
                            break;
                        }
                    }

                    if (selectedShape != null)
                    {
                        scaleShape = true;
                        previousPoint = e.Location;                        
                    }
                    drawing_panel.Invalidate();
                }
               
            }            
        }

        private void drawing_panel_MouseMove(object sender, MouseEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmap))
            {


                if (moving)
                {
                    var d = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);
                    selectedShape.Move(d);
                    previousPoint = e.Location;
                    drawing_panel.Invalidate();
                }
                else if (scaleShape)
                {
                    var newRadius = e.X - previousPoint.X;
                    var dx = e.X - previousPoint.X;
                    var dy = e.Y - previousPoint.Y;                    
                    selectedShape.Resize(e.Location, previousPoint);
                    drawing_panel.Invalidate();
                }            
            }
        }

        private void drawing_panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                selectedShape = null;
                moving = false;
            } else if (scaleShape)
            {
                scaleShape = false;
                selectedShape = null;
            } else if (drawCir)
            {
                //Circle newCircle = new Circle();
                //newCircle.FillColor = brush_color.Color;
                //newCircle.Center = new Point(e.X, e.Y);
                //newCircle.Radius = trackBar1.Value;
                //Shapes.Add(newCircle);
                //drawing_panel.Invalidate();
            }
            else if (drawLine)
            {                
                Line newLine = new Line();
                newLine.LineColor = brush_color.Color;
                newLine.LineWidth = trackBar1.Value;
                newLine.DashPattern = new float[] { 1.0F};
                newLine.Point1 = previousPoint;
                newLine.Point2 = e.Location;
                Shapes.Add(newLine);
            } else if (drawDotted)
            {
                Line newLine = new Line();
                newLine.LineColor = brush_color.Color;
                newLine.LineWidth = trackBar1.Value;
                newLine.DashPattern = new float[] { 2.0F , 2.0F};
                newLine.Point1 = previousPoint;
                newLine.Point2 = e.Location;
                Shapes.Add(newLine);
            } else if (drawRec)
            {                
                Rec newRec = new Rec();
                if (fill)
                {
                    newRec.Fill = true;
                }
                newRec.FillColor = brush_color.Color;
                
                if (e.X < previousPoint.X)
                {
                    newRec.Left = e.X;
                    newRec.Right = previousPoint.X;
                } else
                {
                    newRec.Left = previousPoint.X;
                    newRec.Right = e.X;
                }                                                          

                if (e.Y < previousPoint.Y)
                {
                    newRec.Top = e.Y;
                    newRec.Bottom = previousPoint.Y;
                } else
                {
                    newRec.Top = previousPoint.Y;
                    newRec.Bottom = e.Y;
                }
                                          
                Shapes.Add(newRec);                                
            }
            drawing_panel.Invalidate();
        }

        private void circle_button_Click(object sender, EventArgs e)
        {
            drawCir = true;
            drawRec = false;
            drawLine = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
            fill = false;
            //draw_x = false;
            //update_preview();
        }
        private void line_button_Click(object sender, EventArgs e)
        {
            drawLine = true;
            drawCir = false;            
            drawRec = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
            fill = false;
        }

        private void dotted_line_button_Click(object sender, EventArgs e)
        {
            drawDotted = true;
            drawLine = false;
            drawCir = false;
            drawRec = false;
            eraser = false;
            drawText = false;
            fill = false;
        }


        private void eraser_button_Click(object sender, EventArgs e)
        {
            eraser = true;
            drawCir = false;
            drawLine = false;
            drawRec = false;
            drawText = false;
            drawDotted = false;
            fill = false;
        }


        private void rectangle_button_Click(object sender, EventArgs e)
        {
            drawRec = true;
            drawCir = false;
            drawLine = false;
            drawText = false;
            drawDotted = false;
            fill = false;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            drawCir = false;
            drawLine = false;
            drawRec = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
            fill = false;

        }

        private void text_button_Click(object sender, EventArgs e)
        {
            drawCir = false;
            drawLine = false;
            drawRec = false;
            eraser = false;
            drawText = true;
            drawDotted = false;
            fill = false;
        }

        private void fill_circle_button_Click(object sender, EventArgs e)
        {
            drawCir = true;
            fill = true;
            drawLine = false;
            drawRec = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
        }

        private void fill_rectangle_button_Click(object sender, EventArgs e)
        {
            drawCir = false;
            fill = true;
            drawLine = false;
            drawRec = true;
            eraser = false;
            drawText = false;
            drawDotted = false;
        }


        private void clear_all_button_Click(object sender, EventArgs e)
        {
            Shapes.Clear();
            drawing_panel.Invalidate();
        }

        private void black_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Black;
            update_preview();
        }

        private void undo_button_Click(object sender, EventArgs e)
        {
            
            if (Shapes.Count > 0)
            {
                lastPopped = Shapes.Last();
                Shapes.RemoveAt(Shapes.Count - 1); drawing_panel.Invalidate();
            }
            drawing_panel.Invalidate();
        }
        // need a redo list/stack
        private void redoButton_Click(object sender, EventArgs e)
        {
            Shapes.Add(lastPopped);
            drawing_panel.Invalidate();
        }

        private void red_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Red;
            update_preview();
        }     

        private void green_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Green;
            update_preview();
        }

        private void blue_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Blue;
            update_preview();
        }      

        void update_preview()
        {

        }

        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Enter)
            {
                foreach (Control ctrl in drawing_panel.Controls)
                {
                    if (ctrl.Name == "textbox1")
                    {
                        text_to_draw = ctrl.Text;
                        drawing_panel.Controls.Remove(ctrl);
                    }
                }

                TextInput newText = new TextInput();
                newText.Text = text_to_draw;
                newText.Brush = brush_color;
                newText.Width = 100;
                newText.Height = 25;
                newText.PosX = previousPoint.X;
                newText.PosY = previousPoint.Y;
                Shapes.Add(newText);
                drawing_panel.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            ms = new MemoryStream();
            //save image to memorystream
            bmap.Save(ms, ImageFormat.Bmp);
            bmap = new Bitmap(ms);

                

            //write memory stream to file
            using (FileStream file = new FileStream(@"C:\Users\Kevin\test.bmp", FileMode.Create, FileAccess.Write))
            {
                //byte[] bytes = new byte[ms.Length];
                //ms.Read(bytes, 0, (int)ms.Length);
                //file.Write(bytes, 0, bytes.Length);                    
                ms.CopyTo(file);
            }            
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //using (FileStream file = new FileStream(@"C:\Users\Kevin\test.bmp", FileMode.Open, FileAccess.Read))
            //{
            //    ms = new MemoryStream();
            //    byte[] bytes = new byte[file.Length];
            //    file.Read(bytes, 0, (int)file.Length);
            //    ms.Write(bytes, 0, (int)file.Length/2);

            //    ms.Seek(0, SeekOrigin.Begin);                
            //    bmap = new Bitmap(ms);
            //}
            //drawing_panel.Invalidate();
        }
    }
}
