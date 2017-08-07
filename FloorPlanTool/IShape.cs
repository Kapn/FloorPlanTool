using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    public interface IShape
    {
        IShape Copy();
        GraphicsPath GetPath();
        bool HitTest(Point p);
        void Draw(Graphics g);
        void Move(Point d);
        void Resize(Point e, Point previousPoint);
        string ToString();
        List<int> GetProperties();

    }
}
