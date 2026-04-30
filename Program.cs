using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoiceChat.Forms;
using DotNetEnv;
using VoiceChat.Utils;

namespace VoiceChat
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Env.Load();

            var mainForm = new MainForm();
  
            ITcpManager tcp = new TcpManager();

            Logger.Instance.Init("client");

            Application.Run(mainForm);

            Logger.Instance.Close();

        }
    }
}
