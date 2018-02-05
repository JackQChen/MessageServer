namespace FormsDesigner
{
    using System.ComponentModel.Design.Serialization;

    public interface IDesignerLoaderProvider
    {
        DesignerLoader CreateLoader(IDesignerGenerator generator);
    }
}

