using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloorPlanTool
{
    public partial class Form1 : Form
    {
        /*
         * Variable Declarations 
         */
        bool drawCir, drawLine,drawRec,eraser, scaleShape;
        private SolidBrush brush_color;
        private Color prev_color;
        Bitmap bmap;
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
                        Circle newCircle = new Circle();
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
                    //rectangle
                    else if (drawRec)
                    {
                        previousPoint = e.Location;
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
                // resize with mouse click or right click
                else
                {
                    
                    for (var i = Shapes.Count - 1; i >= 0; i--)
                    {
                        Console.WriteLine("type:" + Shapes[i].ToString());
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
                    selectedShape.Resize(newRadius);
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
            } else if (drawLine)
            {                
                Line newLine = new Line();
                newLine.LineColor = brush_color.Color;
                newLine.LineWidth = trackBar1.Value;
                newLine.Point1 = previousPoint;
                newLine.Point2 = e.Location;
                Shapes.Add(newLine);
            } else if (drawRec)
            {
                Rec newRec = new Rec();
                newRec.FillColor = brush_color.Color;
                var dx = e.X - previousPoint.X;
                var dy = e.Y - previousPoint.Y;
                newRec.Left = previousPoint.X;
                newRec.Right = e.X;
                newRec.Top = previousPoint.Y;
                newRec.Bottom = e.Y;
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
            //text = false;
            //draw_x = false;
            //update_preview();
        }
        private void line_button_Click(object sender, EventArgs e)
        {
            drawLine = true;
            drawCir = false;            
            drawRec = false;
            eraser = false;
        }

        private void eraser_button_Click(object sender, EventArgs e)
        {
            eraser = true;
            drawCir = false;
            drawLine = false;
            drawRec = false;
        }


        private void rectangle_button_Click(object sender, EventArgs e)
        {
            drawRec = true;
            drawCir = false;
            drawLine = false;
            
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            drawCir = false;
            drawLine = false;
            drawRec = false;
            eraser = false;
            
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
    }
}
