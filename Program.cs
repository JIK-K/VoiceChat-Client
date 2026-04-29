using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoiceChat.Forms;
using DotNetEnv;

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

#if DEBUG
            // 테스트 모드: TestTcpManager 사용
            ITcpManager tcp = new TcpManager();
#else
        // 실제 모드: 팀원 TcpManager 사용
        ITcpManager tcp = new TcpManager();
#endif

            Application.Run(mainForm);
           
        }
    }
}
