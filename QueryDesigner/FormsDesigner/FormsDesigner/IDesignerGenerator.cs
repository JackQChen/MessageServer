namespace FormsDesigner
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public interface IDesignerGenerator
    {
        void Attach(FormsDesignerViewContent viewContent);
        void Detach();
        ICollection GetCompatibleMethods(EventDescriptor edesc);
        ICollection GetCompatibleMethods(EventInfo edesc);
        bool InsertComponentEvent(IComponent component, EventDescriptor edesc, string eventMethodName, string body, out string file, out int position);
        void MergeFormChanges(CodeCompileUnit unit);

        System.CodeDom.Compiler.CodeDomProvider CodeDomProvider { get; }
    }
}

