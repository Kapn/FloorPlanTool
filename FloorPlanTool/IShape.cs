using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    [Serializable]
    public abstract class IShape
    {
        public abstract IShape Copy();
        public abstract GraphicsPath GetPath();
        public abstract bool HitTest(Point p);
        public abstract void Draw(Graphics g);
        public abstract void Move(Point d);
        public abstract void Resize(Point e, Point previousPoint);
        public abstract override string ToString();        
    }
}
