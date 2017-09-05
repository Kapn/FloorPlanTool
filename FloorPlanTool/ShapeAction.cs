using System;
using System.Collections.Generic;
using System.Text;

namespace FloorPlanControl
{
    
    public class ShapeAction
    {
        /// <summary>
        /// ShapeAction Constructor
        /// </summary>
        /// <param name="TypeOfAction">Draw, Move, or Resize</param>
        /// <param name="Key"></param>
        /// <param name="Shape">IShape Object</param>        
        public ShapeAction(string TypeOfAction, int Key, IShape Shape)
        {
            this.TypeOfAction = TypeOfAction;
            this.Key = Key;
            this.Shape = Shape;
        }

        public string TypeOfAction { get; set; }
        public int Key { get; set; }
        public IShape Shape{get; set;}
        public bool Updated { get; set; }

    }
}
