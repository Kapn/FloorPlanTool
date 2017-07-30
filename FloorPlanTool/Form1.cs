﻿//Question for Meeting
//-What are you guys using currently for mapping out FloorPlans?

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
using MySql.Data.MySqlClient;

#region TASKS

// TODO: 

// - Undo resize of triangle doesn't draw the triangle
// - Adjust text to be on one-line initially not after resize
// - Text doesn't get redone
// - After erasing first drag of a shape doesn't draw, second time does
// - With many shapes on the board, undo shape manipulation does not work correctly...
// - Undo reverts colored shapes back to black brush color
// - Resizing rectangle past nothing should mirror across

// minor TODO:
// - Grey out 'Clear All' if there are no shapes drawn
// - TextInput makes a 'Ding' windows sound??
// - Resizing shapes starts from smallest possible circle
// - When textbox appears it should be focused ready to be typed in (not have to click again)
// - Clicking places with textbox tool makes text appear at last place clicked.
// - With one shape, sequencing undo and redo over and over for some reason alternates 'highlighting' dotted line tool and eraser
// - File save/load adjusted to a database that is going to be used

// FINISHED (7/24-7/30):
// - Function for check hit-test on each shape. For-loop is in several places
// - Handle z-axis issues for selecting shapes
// - either select most recently drawn shape <-- this is implemented as of now
// - or make all shapes selected by clicking on lines and not inside them.
// - Change UI button images  (need dashed-line image still)
// - Have to reselect text button to type again
// - Escape key should remove textbox control
// - Base resize of triangle of of dY not dX
// - Undo/redo works as intended when there is a single shape.
// - Don't need GetProperties in Copy() methods
// - Undo erase
// - Undo/redo Object Manipulation (move/resize)
// - Shape.Copy() & Shape.GetProperties()
// - Created Actions List filled with ShapeActions
// - Change shape storage from List to Dict

// optional features to implement:
// - erase as holding down mouse
// - multiple triangles, maybe a dropdown button
// - rectangle around shape that shows which shape is selected.

#endregion




namespace FloorPlanTool
{
    public partial class Form1 : Form
    {
        #region Variable Declarations
        /*
         * Variable Declarations 
         */

        //List of Shapes that are to be drawn each time Paint Event gets fired.       
        Dictionary<int, IShape> ShapesDict = new Dictionary<int, IShape>();
        //shapeCount is used as the key of ShapesDict
        int shapeCount = 0;                
        bool drawCir, drawLine,textbox_IsDrawn, drawRec, drawText, drawTri, drawDotted, eraser, fill, scaleShape, moving, just_cleared;
        string text_to_draw;
        SolidBrush brush_color;
        KeyValuePair<int, IShape> selectedShape { get; set; }
        List<ShapeAction> Actions = new List<ShapeAction>();
        Stack<ShapeAction> redo_stack = new Stack<ShapeAction>();                
        Stack<IShape> clear_all_stack = new Stack<IShape>();
        Point previousPoint = Point.Empty;
        TextBox txtbox;
        #endregion

        #region Methods

        /*
         * Form Constructor:
         * Initializes brush to black and enables DoubleBuffering
         */
        public Form1()
        {
            InitializeComponent();

            // Enables double-buffering. Fixes screen flashing when dragging/resizing and ultimately seems to make
            // drawing appear smoother.
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, drawing_panel, new object[] { true });   
                        
            // Initialize brush color to be Black
            brush_color = new SolidBrush(Color.Black);            
        }
       
        /*
         *  OnPaint Event, Fired with calls to drawing_panel.Invalidate()
         *  
         *  Loops through Shapes calling each IShape's Draw method
         */
        private void drawing_panel_Paint(object sender, PaintEventArgs e)
        {
            //handle button enabling
            if (redo_stack.Count <= 0)
            {
                redo_button.Enabled = false;
            }
            else
            {
                redo_button.Enabled = true;
            }                            

            if (ShapesDict.Count <= 0)
            {
                clear_all_button.Enabled = false;
            } else
            {
                clear_all_button.Enabled = true;
            }

            if (Actions.Count <= 0)
            {
                undo_button.Enabled = false;                
            }
            else
            {
                undo_button.Enabled = true;                
            }



            //TESTING UNDO/REDO

            redoTb.Text = "redo_stack\n";
            foreach (var str in redo_stack)
            {
                redoTb.Text += str.Shape.ToString() + "," + str.TypeOfAction + "\n";
            }
            undoTb.Text = "Actions\n";
            foreach (var str in Actions)
            {
                undoTb.Text += str.Shape.ToString() + "," + str.TypeOfAction + "\n";
            }
            shapesDictTb.Text = "ShapesDict\n";
            foreach( var obj in ShapesDict)
            {
                shapesDictTb.Text += obj.Key + " , " + obj.Value.ToString() + "\n";
            }


            //END TESTING

            //enable AntiAlias - fills in pixels along the drawing path to give a smoother appearance
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                       
            //draw each shape in ShapesDict
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
                    newCircle.Radius = 2;
                    
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
                    newLine.LineWidth = 2;
                    newLine.DashPattern = new float[] { 1.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                   
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
                    newLine.LineWidth = 2;
                    newLine.DashPattern = new float[] { 2.0F, 2.0F };
                    newLine.Point1 = previousPoint;
                    newLine.Point2 = e.Location;
                   
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
                    //foreach (KeyValuePair<int, IShape> shape in ShapesDict)
                    //{
                    //    //if a shape was clicked on, erase it.
                    //    if (shape.Value.HitTest(e.Location))
                    //    {
                    //        selectedShape = shape;
                    //        if (selectedShape.Value != null)
                    //        {
                    //            Actions.Add(new ShapeAction("Erase", shape.Key, shape.Value));
                    //            ShapesDict.Remove(shape.Key);
                    //        }
                    //        break;
                    //    }
                    //}
                    bool hit = perform_HitTest(e.Location);

                    if (hit)
                    {
                        Actions.Add(new ShapeAction("Erase", selectedShape.Key, selectedShape.Value));
                        ShapesDict.Remove(selectedShape.Key);
                    }
                }
                //selector
                else
                {                         
                    bool hit = perform_HitTest(e.Location);

                    // if HitTest found a shape to resize:
                    if (hit)
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
                bool hit = perform_HitTest(e.Location);

                // if HitTest found a shape to resize:                
                if (hit)
                {
                    scaleShape = true;
                    previousPoint = e.Location;                    
                    Actions.Add(new ShapeAction("Resize", shapeCount, selectedShape.Value.Copy()));
                }
                drawing_panel.Invalidate();
            }            
            //clear the redo_stack because you shouldn't be able to redo after a Draw
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
                
                    //for viewing line/shapes as they are dragged out
                    if (selectedShape.Value == null)
                    {                        
                        selectedShape = new KeyValuePair<int, IShape>(shapeCount, ShapesDict[shapeCount].Copy());                                                
                    }                    
                    ShapesDict[selectedShape.Key].Resize(e.Location, previousPoint);
                    //selectedShape.Value.Resize(e.Location, previousPoint);
                    drawing_panel.Invalidate();                
                }            
            
        }

        /*
         * OnMouseUp: Draws desired shape and adds it to the Shapes List
         */
        private void  drawing_panel_MouseUp(object sender, MouseEventArgs e)
        {            
            if (moving)
            {
                selectedShape = new KeyValuePair<int, IShape>(shapeCount, null);                
                moving = false;
            } else if (scaleShape)
            {
                scaleShape = false;
                selectedShape = new KeyValuePair<int, IShape>(shapeCount, null);                
            }
            drawing_panel.Invalidate();
        }
               
        /*
         * When Undo is clicked, remove the last shape added to the Shapes List
         * and push it onto the redo_stack. Handle if 'Clear All' was clicked.
         * 
         * TODO: DO I NEED A REDO_STACK?!?! can I just use the actions list and shapesdict instead?
         */
        private void undo_button_Click(object sender, EventArgs e)
        {
            // check if there are any actions to undo
            if (Actions.Count > 0)
            {
                //lookup last action
                ShapeAction lastAction = Actions.Last();                                

                // if last action was an Draw then remove shape from dict to undo the "Draw" Action
                if (lastAction.TypeOfAction == "Draw")
                {
                    //save last action to a redo stack                
                    redo_stack.Push(lastAction);

                    ShapesDict.Remove(lastAction.Key);
                }                
                else if (lastAction.TypeOfAction == "Resize")
                {                                                          
                    //store initial size of shape in redo_stack
                    redo_stack.Push(new ShapeAction("Resize", lastAction.Key, ShapesDict[lastAction.Key]));
                    //set shape back to size stored in lastAction                    
                    ShapesDict[lastAction.Key] = lastAction.Shape;
                }else
                {// else revert ShapesDict back to the state that lastAction saved                 
                    ShapesDict[lastAction.Key] = lastAction.Shape;
                    //save last action to a redo stack                
                    redo_stack.Push(lastAction);
                }                

                //pop last action from actions list
                Actions.RemoveAt(Actions.Count - 1);
                
            } // Add all shapes from the clear_all_stack back into the dict
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
            if (redo_stack.Count > 0)
            {                
                ShapeAction ra = redo_stack.Pop();
                
                //Wrapped in a Try/Catch because Spamming Redo can try to add multiple shapes to the same key
                // ^ can't replicate...
                try
                {
                    if (ra.TypeOfAction == "Erase")
                    {
                        ShapesDict.Remove(ra.Key);
                        Actions.Add(ra);
                    }
                    else if (ra.TypeOfAction == "Resize" || ra.TypeOfAction == "Move")
                    {                        
                        ShapesDict[ra.Key] = ra.Shape;
                        

                        Console.WriteLine("ra.TypeOfAction: " + ra.TypeOfAction);
                        ShapeAction redo_action = new ShapeAction(ra.TypeOfAction, shapeCount, ra.Shape);

                        //add redo_action back to Actions List
                        Actions.Add(redo_action);
                    } else
                    {
                        ShapesDict.Add(shapeCount, ra.Shape);
                        //add redo_action back to Actions List
                        Actions.Add(ra);
                    }

                } catch(Exception err)
                {
                    Console.WriteLine("Spamming Redo too fast, error: " + err);
                }                
            }
            
            drawing_panel.Invalidate();
        }

        /*
         * ClearAll Button Click: Clears all drawings and adds them to
         * clear_all_stack so the 'Clear' Action can be undone if wanted.
         */
        private void clear_all_button_Click(object sender, EventArgs e)
        {
            clear_all_stack.Clear();
            redo_stack.Clear();
            Actions.Clear();
            selectedShape = new KeyValuePair<int, IShape>(100, null);

            shapeCount = 0;

            // For each shape, add it to clear_all_stack.
            foreach (KeyValuePair<int, IShape> shape in ShapesDict)
            {
                clear_all_stack.Push(shape.Value);
            }

            just_cleared = true;
            ShapesDict.Clear();
            drawing_panel.Invalidate();
        }

        /*
         * Loops through ShapesDict to determine which shape is being selected.
         * Sets selectedShape.
         */
        private bool perform_HitTest(Point p)
        {
            foreach (KeyValuePair<int, IShape> shape in ShapesDict)
            {
                if (shape.Value.HitTest(p))
                {
                    //selectedShape = shape;
                    selectedShape = new KeyValuePair<int, IShape>(shape.Key, shape.Value.Copy());
                }
            }

            if (selectedShape.Value != null)
                return true;

            return false;
        }

        /*
         * Handles the KeyDown event for our dynamically created TextBox
         * 
         * Draws the text entered, and removes the textbox from the form                 
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
               
                ShapesDict.Add(++shapeCount, newText);
                Actions.Add(new ShapeAction("Draw", shapeCount, newText));

                textbox_IsDrawn = false;
                drawing_panel.Invalidate();

            }else if (e.KeyCode == Keys.Escape)
            {
                foreach (Control ctrl in drawing_panel.Controls)
                {
                    if (ctrl.Name == "textbox1")
                    {                        
                        drawing_panel.Controls.Remove(ctrl);
                    }
                }
            }                      
        }
        #endregion

        #region Bool Handling for Tool Selection
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

        #endregion

        #region Color selectors
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

        private void black_button_Click(object sender, EventArgs e)
        {
            brush_color.Color = Color.Black;
        }
        #endregion

        #region Load/Save Methods
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
#endregion

    }
}
