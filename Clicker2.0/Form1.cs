using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker2._0
{
    public partial class Clicker : Form
    {
        public string PathInst = File.ReadAllText(@"C:\Users\User\AppData\Local\KLXTEMP\path.txt");
        private const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08; // Нажатие правой кнопки мыши
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        // Модификаторы и идентификаторы
        private const uint MOD_NONE = 0x0000; // Ctrl
        private const uint MOD_CONTROL = 0x0002; // Ctrl
        private const uint MOD_SHIFT = 0x0004; // Ctrl
        private const uint MOD_WIN = 0x0008; // Ctrl
        private const uint MOD_F6 = 0x0117;
        //private const uint MOD = 0x0000;// Ctrl
        private const int HOTKEY_ID = 1;        // ID горячей клавиши

        private bool isClicking = false; // Флаг состояния автокликера
        private bool isClickingKey = false;
        private Thread clickerThread;
        private Thread clickerKeyThread;

        string MouseKey;
        string times;
        string bt1;
        string bt2;
        string bt3;
        string bt4;
        int b = 0;
                        
        public Clicker()
        {
            InitializeComponent();
            //string MouseKey = File.ReadAllText($@"{PathInst}KladovX Project\programs\keymouse.txt");
            //string times = File.ReadAllText($@"{PathInst}KladovX Project\programs\times.txt");
            uint MOD = 0x0000;
            string hotkey = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\hotkey.txt");
            string MouseKey = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\keymouse.txt");
            string times = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt");
            guna2TextBox1.Text = times;
            guna2TextBox2.Text = hotkey;
            if (MouseKey != null)
            {
                if (MouseKey == "Left")
                {
                    guna2ComboBox2.SelectedIndex = 0;
                }
                if (MouseKey == "Right")
                {
                    guna2ComboBox2.SelectedIndex = 1;
                }
            }
            else
            {
                Console.WriteLine("MouseKey is null!");
            }
            Console.WriteLine($"Times: {times}, key: {MouseKey}, HotKey: {hotkey}");
            string[] text = hotkey.Split('+');
            bool registered;
            if (text.Length > 0)
            {
                int lox = (int)Enum.Parse(typeof(Keys), text[1]);
                if (text[0] == "CTRL")
                {
                    MOD = MOD_CONTROL;
                }
                else if (text[0] == "SHIFT")
                { MOD = MOD_SHIFT; }
                else if (text[0] == "WIN")
                { MOD = MOD_WIN; }
                else
                { MOD = MOD_NONE; }
                registered = RegisterHotKey(this.Handle, HOTKEY_ID, MOD, (uint)lox);
            }
            else
            {
                int lox = (int)Enum.Parse(typeof(Keys), text[0]);
                registered = RegisterHotKey(this.Handle, HOTKEY_ID, MOD_NONE, (uint)lox);
            }
            if (!registered)
            {
                MessageBox.Show("Не удалось зарегистрировать горячую клавишу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text == "Mouse Clicker")
            {
                guna2Panel1.Show();
            }
            else
            {
                guna2Panel1.Hide();
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                if (guna2ComboBox1.SelectedIndex == 0)
                { 
                    // Включаем или выключаем автокликер
                    isClicking = !isClicking;

                    if (isClicking)
                    {
                        StartClicker();
                    }
                    else
                    {
                        StopClicker();
                    }
                }
                if (guna2ComboBox1.SelectedIndex == 1)
                {
                    Console.WriteLine("Ye321s!");
                    isClickingKey = !isClickingKey;

                    if (isClickingKey)
                    {
                        StartClickerKey();
                        Console.WriteLine("Yes123!");
                    }
                    else
                    {
                        StopClickerKey();
                    }
                }
            }
        }
        private void StartClicker()
        {
            string MouseKey = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\keymouse.txt");
            string times = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt");
            if (MouseKey != "Left" && MouseKey != "Right")
            {
                MessageBox.Show(MouseKey);
                MessageBox.Show("Указана неправельная кнопка мыши. Попробуйте написать в поле Key: Left или Right" + MouseKey);
            }
            clickerThread = new Thread(() =>
            {
                while (isClicking)
                {
                    try
                    {
                        int times_Int = int.Parse(times);
                        // Имитируем левый клик мыши
                        if (MouseKey == "Left")
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                        if (MouseKey == "Right")
                        {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                        }
                        Task.Delay(times_Int).Wait(); // Интервал между кликами (100 мс)
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка");
                        StopClicker();
                    }
                }
            })
            {
                IsBackground = true
            };
            clickerThread.Start();
        }

        private void StopClicker()
        {
            isClicking = false;
            clickerThread?.Join(); // Завершаем поток
        }
        private void StartClickerKey()
        {
            Console.WriteLine("Yes!");
            string times = File.ReadAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt");
            clickerKeyThread = new Thread(() =>
            {
                while (isClickingKey)
                {
                    try
                    {
                        int times_Int = int.Parse(times);
                        // Имитируем левый клик мыши
                        SendKeys.SendWait(bt1);
                        SendKeys.SendWait(bt2);
                        SendKeys.SendWait(bt3);
                        SendKeys.SendWait(bt4);
                        Task.Delay(times_Int).Wait(); // Интервал между кликами (100 мс)
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка");
                        StopClickerKey();
                    }
                }
            })
            {
                IsBackground = true
            };
            clickerKeyThread.Start();
        }

        private void StopClickerKey()
        {
            isClickingKey = false;
            clickerKeyThread?.Join(); // Завершаем поток
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Receipt: {Your Button}\nOR\nCTRL/WIN/ALT+{Your button}", "Info", MessageBoxButtons.OK);
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\keymouse.txt",guna2ComboBox2.Text);
            MouseKey = guna2ComboBox2.Text;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt", guna2TextBox1.Text);
            times = guna2TextBox1.Text;
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(guna2TextBox2.Text);
            File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\hotkey.txt", guna2TextBox2.Text);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            StartClicker();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            StopClicker();
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine(guna2TextBox2.Text);
            File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\hotkey.txt", guna2TextBox2.Text);
        }

        private void guna2TextBox8_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText($@"{PathInst}KladovX Project\programs\clicker\times.txt", guna2TextBox8.Text);
            times = guna2TextBox8.Text;
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            bt1 = guna2TextBox3.Text;
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            bt2 = guna2TextBox4.Text;
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            bt3 = guna2TextBox5.Text;
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
            bt4 = guna2TextBox6.Text;
        }
        //0; 82
    }
}
