namespace MessageServer.Logging
{

    /// <summary>
    /// ServerLogHandler
    /// </summary>
    /// <param name="name">The logger name</param>
    /// <param name="level">The logger level</param>
    /// <param name="log">The log text</param>
    public delegate void ServerLogHandler(string name, string level, string log);

}
