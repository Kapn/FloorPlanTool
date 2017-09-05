using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Runtime.Serialization;

namespace FloorPlanControl
{
    public interface IShape : ISerializable
    {
        IShape Copy();
        GraphicsPath GetPath();
        bool HitTest(Point p);
        void Draw(Graphics g);
        void Move(Point d);
        void Resize(Point e, Point previousPoint);
        string ToString();        
    }
}
