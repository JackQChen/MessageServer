namespace MessageServer.Extension
{
    static class FormatFileSize
    {
        public static string ToFileSize(this long fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00}GB", fileSize / (1024d * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00}MB", fileSize / (1024d * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00}KB", fileSize / 1024d);
            else
                return string.Format("{0}B", fileSize);
        }
    }
}
