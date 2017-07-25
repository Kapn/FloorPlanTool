using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanTool
{
    public class ShapeAction
    {
        public ShapeAction(string TypeOfAction, int Key, IShape Shape)
        {
            this.TypeOfAction = TypeOfAction;
            this.Key = Key;
            this.Shape = Shape;
        }

        public string TypeOfAction { get; set; }
        public int Key { get; set; }
        public IShape Shape{get; set;}        

    }
}
