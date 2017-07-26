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
// - undo/redo is buggy when combining the two function
// - after erasing first drag of a shape doesn't draw, second time does
// - resizing shapes starts from smallest possible circle
// - don't need GetProperties in Copy() methods
// - undo resize of triangle doesn't draw the triangle
// - change UI buttons to be more intuitive  (need dashed-line image still)

// - have to reselect text button to type again
// - erase as holding down mouse
// - file save/load
// - other triangle shapes, maybe a drop-down button
// - function for check hittest on each shape. For-loop is in several places


// minor TODO:
// - resizing rectangle past nothing should mirror across
// - clicking places with textbox tool makes text appear at last place clicked.




// FINISHED:
// - undo erase
// - undo/redo Object Manipulation (move/resize)
// - change shape storage from List to Dict



namespace FloorPlanTool
{
    public partial class Form1 : Form
    {
        /*
         * Variable Declarations 
         */

        //List of Shapes that are to be drawn each time Paint Event gets fired.
        public List<IShape> Shapes = new List<IShape>();

        Dictionary<int, IShape> ShapesDict = new Dictionary<int, IShape>();
        int shapeCount = 0;

        List<ShapeAction> Actions = new List<ShapeAction>();
        bool drawCir, drawLine,textbox_IsDrawn, drawRec, drawText, drawTri, drawDotted, eraser, fill, scaleShape, moving, just_cleared;
        string text_to_draw;
        SolidBrush brush_color;
        public KeyValuePair<int, IShape> selectedShape { get; set; }        
        Stack<ShapeAction> redo_stack = new Stack<ShapeAction>();
        Stack<IShape> move_stack = new Stack<IShape>();
        Stack<IShape> resize_stack = new Stack<IShape>();        
        Point previousPoint = Point.Empty;
        TextBox txtbox;
        
        public Form1()
        {
            InitializeComponent();

            //enables doublebuffering. Fixes screen flashing when dragging/resizing
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, drawing_panel, new object[] { true });   
                        
            brush_color = new SolidBrush(Color.Black);            
        }
       
        /*
         *  OnPaint Event, Fired with calls to drawing_panel.Invalidate()
         *  
         *  Loops through Shapes calling each IShape's Draw method
         */
        private void drawing_panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
           
            //foreach (var shape in Shapes)
            //    shape.Draw(e.Graphics);

            foreach (KeyValuePair<int, IShape> shape in ShapesDict)
                shape.Value.Draw(e.Graphics);
        }

        /*
        *  On LeftClick: use current selected tool
        *  On RightClick/MouseWheelClick: resize object
        */
        private void drawing_panel_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                //circle
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
                    newCircle.Radius = 2; //trackBar1.Value;
                    //Shapes.Add(newCircle);
                    ShapesDict.Add(++shapeCount, newCircle);
                    Actions.Add(new ShapeAction("Draw", shapeCount, newCircle));

                    scaleShape = true;
                
                }
                //line
                else if(drawLine)
                {                        
                    previousPoint = e.Location;

                    Line newLine = new Line();
                    newLine.LineColor = brush_color.Color;
                    newLine.LineWidth = 2; //trackBar1.Value;
                    newLine.DashPattern = new float[] { 1.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                   // Shapes.Add(newLine);
                    ShapesDict.Add(++shapeCount, newLine);
                    Actions.Add(new ShapeAction("Draw", shapeCount, newLine));

                    scaleShape = true;
                }
                //dotted line
                else if (drawDotted)
                {
                    previousPoint = e.Location;

                    Line newLine = new Line();
                    newLine.LineColor = brush_color.Color;
                    newLine.LineWidth = 2;// trackBar1.Value;
                    newLine.DashPattern = new float[] { 2.0F, 2.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                   // Shapes.Add(newLine);
                    ShapesDict.Add(++shapeCount, newLine);
                    Actions.Add(new ShapeAction("Draw", shapeCount, newLine));

                    scaleShape = true;
                }
                //rectangle
                else if (drawRec)
                {
                    //previousPoint is the initial mousedown location
                    previousPoint = e.Location;

                    Rec newRec = new Rec();
                    
                    //set fill bool
                    if (fill)
                    {
                        newRec.Fill = true;
                    }                 
                    newRec.FillColor = brush_color.Color;

                    //set initial values of rec here
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

                    //Shapes.Add(newRec);
                    ShapesDict.Add(++shapeCount, newRec);                    
                    Actions.Add(new ShapeAction("Draw", shapeCount, newRec));

                    scaleShape = true;                    
                }
                //triangle
                else if (drawTri)
                {
                    previousPoint = e.Location;
                    var size = 20;

                    Triangle newTriangle = new Triangle();
                    newTriangle.LineColor = brush_color.Color;                    
                    newTriangle.Size = size;
                    newTriangle.Location = e.Location;
                    newTriangle.Points = new PointF[]                   
                    {
                        new PointF(e.X, e.Y),
                        new PointF((float)(e.X + size*Math.Cos(Math.PI/3)),
                                   (float)(e.Y + size*Math.Sin(Math.PI/3))),                                  
                        new PointF((float)(e.X + size*Math.Cos((2*Math.PI)/3)),
                                   (float)(e.Y + size*Math.Sin((2*Math.PI)/3)))    //20 is default length
                    };
                    
                    //Shapes.Add(newTriangle);
                    ShapesDict.Add(++shapeCount, newTriangle);
                    Actions.Add(new ShapeAction("Draw", shapeCount, newTriangle));

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
                        txtbox.TextAlign = HorizontalAlignment.Left;
                        drawing_panel.Controls.Add(txtbox);
                        textbox_IsDrawn = true;

                    }
                    
                }
                //eraser
                else if (eraser)
                {
                    //for (var i = Shapes.Count - 1; i >= 0; i--)
                    //{
                    //    //if a Shape was clicked on, erase it.
                    //    if (Shapes[i].HitTest(e.Location))
                    //    {
                    //        selectedShape = Shapes[i];
                    //        if (selectedShape != null)
                    //        {                                                      
                    //            Shapes.RemoveAt(i);
                    //            Actions.Add(new ShapeAction("Erase", Shapes[i]));
                    //        }
                    //        break;
                    //    }
                    //}                       
                    foreach (KeyValuePair<int, IShape> shape in ShapesDict)
                    {
                        //if a shape was clicked on, erase it.
                        if (shape.Value.HitTest(e.Location))
                        {
                            selectedShape = shape;
                            if (selectedShape.Value != null)
                            {
                                Actions.Add(new ShapeAction("Erase", shape.Key, shape.Value));
                                ShapesDict.Remove(shape.Key);
                            }
                            break;
                        }
                    }

                }
                //selector
                else
                {
                    //for (var i = Shapes.Count - 1; i >= 0; i--)
                    //{
                    //    if (Shapes[i].HitTest(e.Location))
                    //    {
                    //        selectedShape = Shapes[i];
                    //        break;
                    //    }
                    //}
                    foreach (KeyValuePair<int, IShape> shape in ShapesDict)
                    {
                        if (shape.Value.HitTest(e.Location))
                        {
                            selectedShape = shape;
                            break;
                        }
                    }


                    if (selectedShape.Value != null)
                    {
                        moving = true;
                        previousPoint = e.Location;
                        Actions.Add(new ShapeAction("Move", selectedShape.Key, selectedShape.Value.Copy()));
                    }
                    drawing_panel.Invalidate();
                }
            }
            // resize with wheel click or right click
            else
            {                    
                foreach (KeyValuePair<int, IShape> shape in ShapesDict)
                {
                    if (shape.Value.HitTest(e.Location))
                    {
                        selectedShape = shape;
                        break;
                    }
                }

                if (selectedShape.Value != null)
                {
                    scaleShape = true;
                    previousPoint = e.Location;                    
                    Actions.Add(new ShapeAction("Resize", shapeCount, selectedShape.Value.Copy()));
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
                    selectedShape.Value.Move(d);
                
                    previousPoint = e.Location;
                    drawing_panel.Invalidate();
                }
                else if (scaleShape)
                {                    
                    var newRadius = e.X - previousPoint.X;
                    var dx = e.X - previousPoint.X;
                    var dy = e.Y - previousPoint.Y;
                
                    //for viewing line/shapes as they are dragged out
                    if (selectedShape.Value == null)
                    {
                        Console.WriteLine("error, selected shape is null when trying to scale! OR initial scaling is occurring");
                        selectedShape = new KeyValuePair<int, IShape>(shapeCount, ShapesDict[shapeCount]);                        
                        //selectedShape = Shapes.Last<IShape>();
                    }
                    
                    selectedShape.Value.Resize(e.Location, previousPoint);
                    drawing_panel.Invalidate();                
                }            
            
        }

        /*
         * OnMouseUp: Draws desired shape and adds it to the Shapes List
         */
        private void  drawing_panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (Actions.Count > 0)
                undo_button.Enabled = true;

            if (moving)
            {
                selectedShape = new KeyValuePair<int, IShape>(shapeCount, null);
                //selectedShape.Value = null;
                moving = false;
            } else if (scaleShape)
            {
                scaleShape = false;
                selectedShape = new KeyValuePair<int, IShape>(shapeCount, null);
                //selectedShape.Value = null;
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
            eraser = false;
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
            //foreach (IShape shape in Shapes)
            //{
            //    clear_all_stack.Push(shape);
            //    just_cleared = true;
            //}
            //Shapes.Clear();

            shapeCount = 0;
            foreach (KeyValuePair<int, IShape> shape in ShapesDict)
            {
                clear_all_stack.Push(shape.Value);
                just_cleared = true;
            }
            ShapesDict.Clear();
                drawing_panel.Invalidate();
        }
    

        /*
         * When Undo is clicked, remove the last shape added to the Shapes List
         * and push it onto the redo_stack. Handle if 'Clear All' was clicked.
         */
        private void undo_button_Click(object sender, EventArgs e)
        {
            // check if there are any actions to undo
            if (Actions.Count > 0)
            {
                //lookup last action
                var lastAction = Actions.Last();                

                //save last action to a redo stack
                redo_stack.Push(lastAction);            
                
                //re-activate redo_button for clicking
                if (redo_button.Enabled == false)
                {
                    redo_button.Enabled = true;
                }

                // if last action was an Draw then remove shape from dict to undo the "Draw" Action
                if (lastAction.TypeOfAction == "Draw")
                {
                    ShapesDict.Remove(lastAction.Key);
                }                
                else
                {// else revert ShapesDict back to the state that lastAction saved                 
                    ShapesDict[lastAction.Key] = lastAction.Shape;
                }                

                //pop last action from actions list
                Actions.RemoveAt(Actions.Count - 1);                       
            }                   
            //else if(just_cleared)
            //{

            //    foreach(IShape shape in clear_all_stack)
            //    {
            //        //Shapes.Add(shape);
            //        ShapesDict.Add(++shapeCount, shape);
            //    }
                
            //    just_cleared = false;                                
            //}
            drawing_panel.Invalidate();
        }
        
        /*
         * When Redo is clicked: pop redo_stack and add it back to the Shapes List
         */
        private void redoButton_Click(object sender, EventArgs e)
        {
            var redo_action = redo_stack.Pop();
            ShapesDict.Add(++shapeCount, redo_action.Shape);
            Actions.Add(redo_action);
            
            //disable redo_button if there is nothing to redo
            if (redo_stack.Count == 0)
            {
                redo_button.Enabled = false;
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
               // Shapes.Add(newText);
                ShapesDict.Add(++shapeCount, newText);
                Actions.Add(new ShapeAction("Draw", shapeCount, newText));

                textbox_IsDrawn = false;
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
