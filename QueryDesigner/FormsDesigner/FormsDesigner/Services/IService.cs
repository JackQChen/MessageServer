namespace FormsDesigner.Services
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IService
    {
        event EventHandler Initialize;

        event EventHandler Unload;

        void InitializeService();
        void UnloadService();
    }
}

