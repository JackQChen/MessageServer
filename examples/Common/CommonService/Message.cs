using System;

namespace CommonService
{
    public enum MessageType { Text = 0, Data = 1, RawData = 2 }

    [Serializable]
    public abstract class Message : IDisposable
    {
        public MessageType Type { get; set; }

        public Message()
        {
        }

        public virtual void Dispose()
        {
        }
    }

    [Serializable]
    public class TextMessage : Message
    {
        public string Text { get; set; }

        public TextMessage()
        {
            this.Type = MessageType.Text;
        }
    }

    [Serializable]
    public class DataMessage : TextMessage
    {
        public byte[] Data { get; set; }

        public DataMessage()
        {
            this.Type = MessageType.Data;
        }

        public override void Dispose()
        {
            this.Data = null;
        }
    }

    [Serializable]
    public class FileInfoMessage : Message
    {
        public FileInfoMessage()
        {
            this.Type = MessageType.RawData;
        }

        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileMD5 { get; set; }
    }

}
