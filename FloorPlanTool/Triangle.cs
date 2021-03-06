﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    // ---------------------------------
    //  Handles Drawing/Manipulating Triangles
    // ---------------------------------
    // Properties:
    //      Location : Point used as anchor
    //      Points : Array of 3 PointF's that represent each point of the triangle
    //      Size :  int used to scale length of each side of the triangle
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

    class Triangle : IShape
    {   
        public Triangle(Point Location, int Size) {
            this.Location = Location;
            this.Size = Size;
            this.LineWidth = 2;
            this.Points = new PointF[]
                            {
                            new PointF(Location.X, Location.Y),
                            new PointF((float)(Location.X + Size*Math.Cos(Math.PI/3)),
                                        (float)(Location.Y + Size*Math.Sin(Math.PI/3))),
                            new PointF((float)(Location.X + Size*Math.Cos((2*Math.PI)/3)),
                                        (float)(Location.Y + Size*Math.Sin((2*Math.PI)/3)))    //size = 20 by default
                            };
        }
        public Triangle(Point Location, Color LineColor, int Size)
        {
            this.Location = Location;
            //this.Points = Points;
            this.LineColor = LineColor;
            LineWidth = 2;
            this.Points = new PointF[]
                          {
                            new PointF(Location.X, Location.Y),
                            new PointF((float)(Location.X + Size*Math.Cos(Math.PI/3)),
                                        (float)(Location.Y + Size*Math.Sin(Math.PI/3))),
                            new PointF((float)(Location.X + Size*Math.Cos((2*Math.PI)/3)),
                                        (float)(Location.Y + Size*Math.Sin((2*Math.PI)/3)))    //size = 20 by default
                          };
        }

        public Point Location { get; set; }
        public PointF[] Points { get; set; }        
        public int Size { get; set; }
        public int LineWidth { get; set; }
        public Color LineColor { get; set; }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddPolygon(Points);            
            return path;
        }

        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
            using (var pen = new Pen(LineColor, LineWidth))                
                result = path.IsVisible(p);
            return result;
        }

        public void Draw(Graphics g)
        {            
            using (var pen = new Pen(LineColor, LineWidth))
            {                      
                g.DrawPolygon(pen, Points);
            }
        }

        public void Move(Point d)
        {            
            Points[0] = new PointF(Points[0].X + d.X, Points[0].Y + d.Y);
            Points[1] = new PointF(Points[1].X + d.X, Points[1].Y + d.Y);
            Points[2] = new PointF(Points[2].X + d.X, Points[2].Y + d.Y);
            Location = new Point((int)(Points[2].X + d.X), (int)(Points[2].Y + d.Y)); // same as Points[2] adjustment

        }

        public void Resize(Point e, Point distance)
        {                      
            this.Size = e.Y - distance.Y;

            Points[0] = new PointF((float)(Location.X + this.Size * Math.Cos(Math.PI / 3)),
                              (float)(Location.Y + this.Size * Math.Sin(Math.PI / 3)));
            Points[1] = new PointF((float)(Location.X + this.Size * Math.Cos((2 * Math.PI) / 3)),
                                 (float)(Location.Y + this.Size * Math.Sin((2 * Math.PI) / 3)));
            Points[2] = Location;                    

        }           

        public IShape Copy()
        {            
            return new Triangle(Location, LineColor, Size);
        }

        string IShape.ToString()
        {
            return String.Format("Triangle\nSize: {0}, Location: ({1}, {2}), Points: ({3}, {4}, {5})", Size, Location.X, Location.Y, Points[0].X, Points[1].X, Points[2].X);            
        }
    }
}
