using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    public class ShapeAction
    {
        public ShapeAction(string TypeOfAction, IShape Shape)
        {
            this.TypeOfAction = TypeOfAction;
            this.Shape = Shape;
        }

        public string TypeOfAction { get; set; }
        public IShape Shape{get; set;}        

    }
}
