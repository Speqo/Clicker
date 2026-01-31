using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;


namespace Clicker2._0
{

    internal static class Program
    {
        public static bool IsRunningAsAdmin()
        {
            // Получаем текущее удостоверение пользователя
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            // Проверяем, входит ли пользователь в роль администратора
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!IsRunningAsAdmin())
            {
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var startInfo = new System.Diagnostics.ProcessStartInfo(exeName)
                {
                    Verb = "runas" 
                };
                System.Diagnostics.Process.Start(startInfo);
                return; 
            }
            if (File.Exists(@"C:\Users\User\AppData\Local\KLXTEMP\path.txt"))
            {
                Console.WriteLine("File Found path.txt");
            }
            else
            {
                Directory.CreateDirectory(@"C:\Users\User\AppData\Local\KLXTEMP");
                File.WriteAllText(@"C:\Users\User\AppData\Local\KLXTEMP\path.txt", @"C:\Program Files\");
            }
            string PathInst = File.ReadAllText(@"C:\Users\User\AppData\Local\KLXTEMP\path.txt");
            Console.WriteLine($"--{PathInst}--");
            if (File.Exists($@"{PathInst}KladovX Project\know.txt"))
            {
                Console.WriteLine("File found know.txt");
            }
            else
            {
                try
                {
                    Directory.CreateDirectory($@"{PathInst}KladovX Project");
                    Directory.CreateDirectory($@"{PathInst}KladovX Project\programs");
                    Directory.CreateDirectory($@"{PathInst}KladovX Project\programs\clicker");
                    File.WriteAllText($@"{PathInst}KladovX Project\know.txt", String.Empty);
                    File.WriteAllText($@"{PathInst}KladovX Project\library.txt", String.Empty);
                    File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt", "1000");
                    File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\keymouse.txt", "Left");
                    File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\hotkey.txt", "None+F6");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка! {e.Message}");
                }
            }
            if (File.Exists($@"{PathInst}KladovX Project\programs\clicker\clicker.exe"))
            {
                Console.WriteLine($@"clicker found");
            }
            else
            {
                Directory.CreateDirectory($@"{PathInst}KladovX Project\programs\clicker");
                File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt", "1000");
                File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\keymouse.txt", "Left");
                File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\hotkey.txt", "None+F6");
            }
            string filePath = $@"{PathInst}KladovX Project\know.txt";
            Console.WriteLine($"---{filePath}---");
            Application.Run(new Clicker());
        }
    }
}
