#### 调用示例

```C#

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var updatePath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.exe";
            //程序默认为无参数启动
            if (args.Length == 0)
            {
                //如果有自动更新程序，则进行自动更新
                if (File.Exists(updatePath))
                {
                    //启动自动更新程序并将更新完后启动的程序名以参数形式进行传递
                    System.Diagnostics.Process.Start(updatePath, "LEDDisplay.exe");
                    //当前程序退出
                    return;
                }
            }
            //自动更新启动程序时第一个参数为AutoUpdate
            else if (args[1] == "AutoUpdate")
            {
                var newUpdatePath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.exe.tmp";
                //判断自动更新程序自身是否需要更新
                if (File.Exists(newUpdatePath))
                {
                    File.Delete(updatePath);
                    File.Move(newUpdatePath, updatePath);
                }
                //var remotingConfigPath = AppDomain.CurrentDomain.BaseDirectory + "RemotingConfig.xml";
                //第二个参数是本次是否发生过更新
                //有新的更新内容
                //if (bool.Parse(args[2]))
                //{
                //    var config = File.ReadAllText(remotingConfigPath).Replace("0.0.0.0:0000", ConfigurationManager.AppSettings["RemotingConfig"]);
                //    File.WriteAllText(remotingConfigPath, config);
                //}
                //RemotingConfiguration.Configure(remotingConfigPath, false);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
    
```
JackChen<br>
2018-02-05
