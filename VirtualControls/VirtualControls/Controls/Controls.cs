using System.Collections.Generic;

namespace VirtualControls.Controls
{
    public class Controls : List<VirtualControl>
    {
        VirtualControlContainer parent;

        public Controls(VirtualControlContainer parent)
        {
            this.parent = parent;
        }

        public new void Add(VirtualControl control)
        {
            control.Parent = this.parent;
            base.Add(control);
        }
    }
}
