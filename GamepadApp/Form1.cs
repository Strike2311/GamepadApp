using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Gaming.Input;
namespace GamepadApp
{
    public partial class Form1 : Form
    {
        obj rec = new obj(DefaultBackColor);
        Gamepad Controller;
        Timer t = new Timer();
        static bool rb = false, lb = false;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            t.Tick += T_Tick;
            t.Interval = 1;
            t.Start();
        }

        private async void T_Tick(object sender, EventArgs e)
        {
            if(Gamepad.Gamepads.Count > 0)
            {
                Controller = Gamepad.Gamepads.First();
                var Reading = Controller.GetCurrentReading();
                switch (Reading.Buttons)
                {
                    case GamepadButtons.RightShoulder:
                        await Log("Right Bumper Pressed");
                        rb = true;
                        break;
                    case GamepadButtons.LeftShoulder:
                        await Log("Left Bumper Pressed");
                        lb = true;
                        break;
                    default:
                        rb = false;
                        lb = false;
                        break;



                }
                if (Reading.LeftThumbstickX > 0.3 || Reading.LeftThumbstickX < -0.3)
                {
                    int x = (int)(Reading.LeftThumbstickX*10 % 11);
                    if (rb == true) {
                        rec.moveObject(this.CreateGraphics(), rec.x += x / 2, rec.y, Brushes.Green, 5 );
                    }
                    else if (lb == true)
                    {
                        rec.moveObject(this.CreateGraphics(), rec.x += x / 2, rec.y, Brushes.Red, 5);
                    }
                    else
                    {
                        rec.moveObject(this.CreateGraphics(), rec.prev_x, rec.prev_y, Brushes.White, 5);
                        rec.moveObject(this.CreateGraphics(), rec.x += x / 2, rec.y, Brushes.Black, 5);
                    }
                    rec.prev_x += x/2;

                }
                if (Reading.LeftThumbstickY > 0.3 || Reading.LeftThumbstickY < -0.3)
                {
                    int y = (int)(Reading.LeftThumbstickY * 10 % 11);

                    if (rb == true)
                    {
                        rec.moveObject(this.CreateGraphics(), rec.x, rec.y -= y / 2, Brushes.Green, 5);
                    }
                    else if (lb == true)
                    {
                        rec.moveObject(this.CreateGraphics(), rec.x, rec.y -= y / 2, Brushes.Red, 5);
                    }
                    else
                    {
                        rec.moveObject(this.CreateGraphics(), rec.prev_x, rec.prev_y, Brushes.White, 5);
                        rec.moveObject(this.CreateGraphics(), rec.x, rec.y -= y / 2, Brushes.Black, 5);
                    }
                    rec.prev_y -= y/2;
                }
            }
        }
        private async void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            await Log("Controller Removed");

        }

        private async void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            await Log("Controller Added");

        }

        private async Task Log(string txt)
        {
            Task t = Task.Run(() =>
            {
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + ": " + txt);
            }
            );
            await t;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
             rec.createShape(this.CreateGraphics(), 200, 200, Brushes.Black, 5);
        }
        class obj
        {
            private Graphics g;
            private Color defaultFormColor;
            public int x;
            public int y;
            private int size;
           public int prev_y = 10, prev_x = 10;

            private Brush b;
            public obj(Color defaultFormColor)
            {
                this.defaultFormColor = defaultFormColor;
            }

            public void createShape(Graphics G, int X, int Y, Brush B, int Size)
            {
                G.FillRectangle(B, X, Y, Size, Size);
                this.x = X;
                this.y = Y;
                this.b = B;
                this.size = Size;
                this.g = G;
            }

            public void moveObject(Graphics G, int x, int y, Brush B, int size)
            {
                createShape(G, x, y, B, size);
               
            }

         


        }
       

       
    }
}
