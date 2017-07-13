/* Notes 7/11 for meeting:
 * Should I remove the trackbar?
 */


using System;
using System.Windows;
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

//used for testing storage in MySQL Database
//using MySql.Data.MySqlClient;

// TODO: 
// - change UI buttons to be more intuitive  (need dashed-line image still)
// - drag triangle for drawing
// - undo erase
// - file save/load

// minor TODO:
// - resizing rectangle past nothing should mirror across
// - undo/redo Object Manipulation (move/resize)
// - clicking places with textbox tool makes text appear at last place clicked.
// - right click drag shouldn't draw rectangles (or any shapes, tri does it too)
// - select inside of triangle for dragging, not having to precisely click the lines




namespace FloorPlanTool
{
    public partial class Form1 : Form
    {
        /*
         * Variable Declarations 
         */        
        
        //List of Shapes that are to be drawn each time Paint Event gets fired.
        public List<IShape> Shapes { get; private set; }        
        bool drawCir, drawLine,textbox_IsDrawn, drawRec, drawText, drawTri, drawDotted, eraser, fill, scaleShape, moving, just_cleared;
        string text_to_draw;
        SolidBrush brush_color;
        IShape selectedShape; 
        Stack<IShape> redo_stack = new Stack<IShape>();
        Point previousPoint = Point.Empty;
        TextBox txtbox;
        
        public Form1()
        {
            InitializeComponent();

            //enables doublebuffering. Fixes screen flashing when dragging/resizing
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, drawing_panel, new object[] { true });   
            
            Shapes = new List<IShape>();
            brush_color = new SolidBrush(Color.Black);            
        }
       
        /*
         *  OnPaint Event, Fired with calls to drawing_panel.Invalidate()
         *  
         *  Loops through Shapes calling each IShape's Draw method
         */
        private void drawing_panel_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImage(bmap, Point.Empty);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
           
            foreach (var shape in Shapes)
                shape.Draw(e.Graphics);
      
        }

        /*
        *  On LeftClick: use current selected tool
        *  On RightClick/MouseWheelClick: resize object
        */
        private void drawing_panel_MouseDown(object sender, MouseEventArgs e)
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

                    scaleShape = true;
                    //drawing_panel.Invalidate();
                }
                //line
                else if(drawLine)
                {                        
                    previousPoint = e.Location;

                    Line newLine = new Line();
                    newLine.LineColor = brush_color.Color;
                    newLine.LineWidth = trackBar1.Value;
                    newLine.DashPattern = new float[] { 1.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                    Shapes.Add(newLine);

                    scaleShape = true;
                }
                //dotted line
                else if (drawDotted)
                {
                    previousPoint = e.Location;

                    Line newLine = new Line();
                    newLine.LineColor = brush_color.Color;
                    newLine.LineWidth = trackBar1.Value;
                    newLine.DashPattern = new float[] { 2.0F, 2.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                    Shapes.Add(newLine);

                    scaleShape = true;
                }
                //rectangle
                else if (drawRec)
                {   
                    if (!scaleShape)
                    { 
                        previousPoint = e.Location;
                    }
                    

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

                    }
                    else
                    {
                        newRec.Left = previousPoint.X;
                        newRec.Right = e.X;
                    }

                    if (e.Y < previousPoint.Y)
                    {
                        newRec.Top = e.Y;
                        newRec.Bottom = previousPoint.Y;
                    }
                    else
                    {
                        newRec.Top = previousPoint.Y;
                        newRec.Bottom = e.Y;
                    }

                    Shapes.Add(newRec);

                    scaleShape = true;
                    Console.WriteLine("MouseDown");
                }
                //triangle
                else if (drawTri)
                {
                    previousPoint = e.Location;

                    Triangle newTriangle = new Triangle();
                    newTriangle.LineColor = brush_color.Color;
                    newTriangle.LineWidth = trackBar1.Value - 2;
                    newTriangle.Point1 = new Point(e.X, e.Y);
                    newTriangle.Point2 = new Point(e.X + 10, e.Y + 15);
                    newTriangle.Point3 = new Point(e.X - 10, e.Y + 15);
                    Shapes.Add(newTriangle);

                    scaleShape = true;
                }
                //draw text
                else if (drawText)
                {
                    //Create a TextBox object and add event.KeyDown method
                    previousPoint = e.Location;
                    if (!textbox_IsDrawn)
                    {
                        txtbox = new TextBox { Name = "textbox1" };
                        txtbox.KeyDown += textbox_KeyDown;
                        txtbox.Location = new Point(e.X, e.Y);
                        txtbox.TextAlign = HorizontalAlignment.Center;
                        drawing_panel.Controls.Add(txtbox);
                        textbox_IsDrawn = true;

                    }
                    
                }
                //eraser
                else if (eraser)
                {
                    for (var i = Shapes.Count - 1; i >= 0; i--)
                    {
                        //if a Shape was clicked on, erase it.
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
            redo_stack.Clear();
        }

        /*
         * MouseMove handles Moving and Resizing shapes
         */
        private void drawing_panel_MouseMove(object sender, MouseEventArgs e)
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
                
                    if (selectedShape == null)
                    {
                        selectedShape = Shapes.Last<IShape>();
                    }
                    selectedShape.Resize(e.Location, previousPoint);
                    drawing_panel.Invalidate();
                Console.WriteLine("mouse moving, scaling shape");
                }            
            
        }

        /*
         * OnMouseUp: Draws desired shape and adds it to the Shapes List
         */
        private void  drawing_panel_MouseUp(object sender, MouseEventArgs e)
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
                //Line newLine = new Line();
                //newLine.LineColor = brush_color.Color;
                //newLine.LineWidth = trackBar1.Value;
                //newLine.DashPattern = new float[] { 1.0F};
                //newLine.Point1 = previousPoint;
                //newLine.Point2 = e.Location;
                //Shapes.Add(newLine);
            }
            else if (drawDotted)
            {
                //Line newLine = new Line();
                //newLine.LineColor = brush_color.Color;
                //newLine.LineWidth = trackBar1.Value;
                //newLine.DashPattern = new float[] { 2.0F , 2.0F};
                //newLine.Point1 = previousPoint;
                //newLine.Point2 = e.Location;
                //Shapes.Add(newLine);
            }
            else if (drawTri)
            {
                //Triangle newTriangle = new Triangle();
                //newTriangle.LineColor = brush_color.Color;
                //newTriangle.LineWidth = trackBar1.Value - 2;
                //newTriangle.Point1 = new Point(e.X, e.Y);
                //newTriangle.Point2 = new Point(e.X + 10, e.Y + 15);
                //newTriangle.Point3 = new Point(e.X - 10, e.Y + 15);                               
                //Shapes.Add(newTriangle);
            }
            else if (drawRec)
            {                
            //    Rec newRec = new Rec();
            //    if (fill)
            //    {
            //        newRec.Fill = true;
            //    }
            //    newRec.FillColor = brush_color.Color;
                
            //    if (e.X < previousPoint.X)
            //    {
            //        newRec.Left = e.X;
            //        newRec.Right = previousPoint.X;
            //    } else
            //    {
            //        newRec.Left = previousPoint.X;
            //        newRec.Right = e.X;
            //    }                                                          

            //    if (e.Y < previousPoint.Y)
            //    {
            //        newRec.Top = e.Y;
            //        newRec.Bottom = previousPoint.Y;
            //    } else
            //    {
            //        newRec.Top = previousPoint.Y;
            //        newRec.Bottom = e.Y;
            //    }
                                          
            //    Shapes.Add(newRec);                                
            }
            drawing_panel.Invalidate();
        }

        /* 
         * Tool Buttons: Handles bools for drawing functions
         */
        private void circle_button_Click(object sender, EventArgs e)
        {
            drawCir = true;
            drawRec = false;
            drawLine = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
            fill = false;
            drawTri = false;
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
            drawTri = false;
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
            drawTri = false;
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
            drawTri = false;
        }


        private void rectangle_button_Click(object sender, EventArgs e)
        {
            drawRec = true;
            drawCir = false;
            drawLine = false;
            drawText = false;
            drawDotted = false;
            fill = false;
            drawTri = false;
        }

        private void tri_button_Click(object sender, EventArgs e)
        {
            drawCir = false;
            drawLine = false;
            drawRec = false;
            eraser = false;
            drawText = false;
            drawDotted = false;
            fill = false;            
            drawTri = true;
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
            drawTri = false;
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
            drawTri = false;
            textbox_IsDrawn = false;
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
            drawTri = false;
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
            drawTri = false;
        }    

        private void black_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Black;           
        }

        Stack<IShape> clear_all_stack = new Stack<IShape>();

        private void clear_all_button_Click(object sender, EventArgs e)
        {
            clear_all_stack.Clear();
            //push onto redo stack
            foreach (IShape shape in Shapes)
            {
                clear_all_stack.Push(shape);
                just_cleared = true;
            }
            Shapes.Clear();
            drawing_panel.Invalidate();
        }

    
        /*
         * When Undo is clicked, remove the last shape added to the Shapes List
         * and push it onto the redo_stack. Handle if 'Clear All' was clicked.
         */
        private void undo_button_Click(object sender, EventArgs e)
        {
            
            if (Shapes.Count > 0)
            {                
                redo_stack.Push(Shapes.Last());
                Shapes.RemoveAt(Shapes.Count - 1);                
            } else if(just_cleared)
            {
                foreach(IShape shape in clear_all_stack)
                {
                    Shapes.Add(shape);
                }
                just_cleared = false;                                
            }
            drawing_panel.Invalidate();
        }
        
        /*
         * When Redo is clicked: pop redo_stack and add it back to the Shapes List
         */
        private void redoButton_Click(object sender, EventArgs e)
        {
            if (redo_stack.Count > 0)
            {
                IShape popped_shape = redo_stack.Pop();
                Shapes.Add(popped_shape);
            }            
            drawing_panel.Invalidate();
        }

        /*
         * Adjust brush_color for each button selected
         */
        private void red_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Red;            
        }     

        private void green_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Green;
            
        }

        private void blue_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Blue;            
        }      

        /*
         * Handles the KeyDown event for our dynamically created TextBox
         * 
         * Draws the text entered, and removes the textbox from the form
         * 
         * TODO: only allow 1 TextBox to be created at a time
         */
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

        //currently saves to file for testing of database in other solution 'ProgrammingKnowledge'
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bitmap bmp = new Bitmap((int)drawing_panel.Width, (int)drawing_panel.Height);
            ////Draw Graphics to Bitmap for storage
            //drawing_panel.DrawToBitmap(bmp, new Rectangle(0, 0, drawing_panel.Width, drawing_panel.Height));            

            ////saved to file to check contents of bmp
            ////using (FileStream saveStream = new FileStream(@"C:\Users\kpannell\testing.png", FileMode.OpenOrCreate))
            ////{
            ////    bmp.Save(saveStream, ImageFormat.Png);
            ////}


            //// save to localhost for testing
            // try
            //{
            //    string myConnection = "datasource=localhost;port=3306;username=root;password=root";
            //    MySqlConnection myConn = new MySqlConnection(myConnection);
            //    myConn.Open();

            //    string cmdText = "INSERT INTO test_schema.file(idfile, file_name, memorystream, file_size) VALUES (@idfile, @filename, @memorystream, @filesize)";
            //    MySqlCommand cmd = new MySqlCommand(cmdText, myConn);
            //    cmd.Parameters.AddWithValue("@idfile", 55);
            //    cmd.Parameters.AddWithValue("@filename", "checkingSize");


            //    //save image to memorystream
            //    using (MemoryStream memStream = new MemoryStream())
            //    {
                    
            //        bmp.Save(memStream, ImageFormat.Bmp);
            //        byte[] ms_Array = memStream.ToArray();
            //        cmd.Parameters.AddWithValue("@filesize", memStream.Length);
            //        cmd.Parameters.AddWithValue("@memorystream", ms_Array);
            //        cmd.ExecuteNonQuery();
            //    }                    
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //bmp.Dispose();
        }

        //load currently only attempts to grab the stream from the database
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //byte[] image = GetImage("55");
            //MemoryStream stream = new MemoryStream(image);

        }

        //Grabs memorystream by fileid field and returns the byte array           
        byte[] GetImage(string id)
        {
            //using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=root"))
            //{
            //    try
            //    {
            //        connection.Open();
            //        MySqlCommand cmd = new MySqlCommand("SELECT memorystream FROM test_schema.file WHERE idfile = @id", connection);
            //        cmd.Parameters.AddWithValue("@id", id);

            //        using (MySqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            reader.Read();
            //            FileStream outfile = new FileStream("testing.bmp", FileMode.OpenOrCreate, FileAccess.Write);

            //            using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(outfile))
            //            {
            //                long bufferSize = reader.GetBytes(0, 0, null, 0, 0);
            //                byte[] buffer = new byte[bufferSize];

            //                reader.GetBytes(0, 0, buffer, 0, (int)bufferSize);
            //                writer.Write(buffer, 0, (int)bufferSize);
            //                writer.Flush();

            //                return buffer;

            //            }
            //        }
            //    }
            //    catch (IndexOutOfRangeException er)
            //    {                    
            //        MessageBox.Show("Error has occurred: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //    return null;
            //}
            return null;
        }
            
    }
}
