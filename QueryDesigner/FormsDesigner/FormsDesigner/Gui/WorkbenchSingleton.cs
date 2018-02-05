namespace FormsDesigner.Gui
{
    using FormsDesigner.Core;
    using FormsDesigner.Services;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class WorkbenchSingleton
    {
        private static STAThreadCaller caller;
        private static IWorkbench workbench = null;

        public static  event EventHandler WorkbenchCreated;

        internal static void AssertMainThread()
        {
            if (InvokeRequired)
            {
                throw new InvalidOperationException("This operation can be called on the main thread only.");
            }
        }

        public static void InitializeWorkbench()
        {
            PropertyService.PropertyChanged += new PropertyChangedEventHandler(WorkbenchSingleton.TrackPropertyChanges);
            caller = new STAThreadCaller((Form) workbench);
            OnWorkbenchCreated();
        }

        private static void OnWorkbenchCreated()
        {
            if (WorkbenchCreated != null)
            {
                WorkbenchCreated(null, EventArgs.Empty);
            }
        }

        public static void SafeThreadAsyncCall(Action method)
        {
            caller.BeginCall(method, new object[0]);
        }

        public static void SafeThreadAsyncCall<A>(Action<A> method, A arg1)
        {
            caller.BeginCall(method, new object[] { arg1 });
        }

        public static void SafeThreadAsyncCall<A, B>(Action<A, B> method, A arg1, B arg2)
        {
            caller.BeginCall(method, new object[] { arg1, arg2 });
        }

        public static void SafeThreadAsyncCall<A, B, C>(Action<A, B, C> method, A arg1, B arg2, C arg3)
        {
            caller.BeginCall(method, new object[] { arg1, arg2, arg3 });
        }

        public static void SafeThreadCall(Action method)
        {
            caller.Call(method, new object[0]);
        }

        public static void SafeThreadCall<A>(Action<A> method, A arg1)
        {
            caller.Call(method, new object[] { arg1 });
        }

        public static void SafeThreadCall<A, B>(Action<A, B> method, A arg1, B arg2)
        {
            caller.Call(method, new object[] { arg1, arg2 });
        }

        public static void SafeThreadCall<A, B, C>(Action<A, B, C> method, A arg1, B arg2, C arg3)
        {
            caller.Call(method, new object[] { arg1, arg2, arg3 });
        }

        public static R SafeThreadFunction<R>(Func<R> method)
        {
            return (R) caller.Call(method, new object[0]);
        }

        public static R SafeThreadFunction<A, R>(Func<A, R> method, A arg1)
        {
            return (R) caller.Call(method, new object[] { arg1 });
        }

        private static void TrackPropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            if ((e.OldValue != e.NewValue) && (workbench != null))
            {
                string key = e.Key;
                if (key != null)
                {
                    if ((!(key == "ICSharpCode.SharpDevelop.Gui.StatusBarVisible") && !(key == "ICSharpCode.SharpDevelop.Gui.VisualStyle")) && !(key == "ICSharpCode.SharpDevelop.Gui.ToolBarVisible"))
                    {
                        if (key == "ICSharpCode.SharpDevelop.Gui.UseProfessionalRenderer")
                        {
                        }
                    }
                    else
                    {
                        workbench.RedrawAllComponents();
                    }
                }
            }
        }

        public static Control ActiveControl
        {
            get
            {
                Control activeControl;
                ContainerControl mainForm = MainForm;
                do
                {
                    activeControl = mainForm.ActiveControl;
                    if (activeControl == null)
                    {
                        return mainForm;
                    }
                    mainForm = activeControl as ContainerControl;
                }
                while (mainForm != null);
                return activeControl;
            }
        }

        public static bool InvokeRequired
        {
            get
            {
                if (workbench == null)
                {
                    return false;
                }
                return ((Form) workbench).InvokeRequired;
            }
        }

        public static Form MainForm
        {
            get
            {
                return (Form) workbench;
            }
        }

        public static IWorkbench Workbench
        {
            get
            {
                return workbench;
            }
            set
            {
                workbench = value;
            }
        }

        private class STAThreadCaller
        {
            private Control ctl;

            public STAThreadCaller(Control ctl)
            {
                this.ctl = ctl;
            }

            public void BeginCall(Delegate method, object[] arguments)
            {
                if (method == null)
                {
                    throw new ArgumentNullException("method");
                }
                this.ctl.BeginInvoke(method, arguments);
            }

            public object Call(Delegate method, object[] arguments)
            {
                if (method == null)
                {
                    throw new ArgumentNullException("method");
                }
                return this.ctl.Invoke(method, arguments);
            }
        }
    }
}

