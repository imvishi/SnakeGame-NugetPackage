using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Activities;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
namespace SnakeGame
{
    public class SnakeGame:CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> SizeOfPlayArea { get; set; }
        
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int>Delay{ get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        //mouse event constants
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        //Coordinatates
        public struct POS
        {
            public uint x;
            public uint y;
        };
        //Create Boundry
        public void CreateBoundry(int size,int delay)
        {
            size+=20;
            int startx = 190;
            int starty = 190;
            //Top boundry
            for (int i = 0; i < size; i++)
            {
                System.Windows.Forms.Cursor.Position = new Point(startx, starty);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)startx, (uint)starty, 0, 0);
                Thread.Sleep(delay);
                startx+=1;
            }
            //Right Side
            for (int i = 0; i < size; i++)
            {
                System.Windows.Forms.Cursor.Position = new Point(startx, starty);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)startx, (uint)starty, 0, 0);
                Thread.Sleep(delay);
                starty+=1;
            }
            //bottom
            for (int i = 0; i < size; i++)
            {
                System.Windows.Forms.Cursor.Position = new Point(startx, starty);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)startx, (uint)starty, 0, 0);
                Thread.Sleep(delay);
                startx-=1;
            }
            //Left
            for (int i = 0; i < size; i++)
            {
                System.Windows.Forms.Cursor.Position = new Point(startx, starty);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)startx, (uint)starty, 0, 0);
                Thread.Sleep(delay);
                starty -= 1;
            }
        }
        protected override void Execute(CodeActivityContext context)
        {

            List<POS> snake = new List<POS>();
            POS rat = new POS();
            Random r = new Random();
            int k = 0;
            int size = SizeOfPlayArea.Get(context);
            int delay = Delay.Get(context);
            if (size<300)
                size=300;

            //Create boundry of PlayArea
            CreateBoundry(size,delay);
            
            //Draw Rat 
            rat.x=(uint)r.Next(200,200+size);
            rat.y=(uint)r.Next(200,200+size);
            System.Windows.Forms.Cursor.Position = new Point((int)rat.x, (int)rat.y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, rat.x,rat.y, 0, 0);
            Thread.Sleep(1000);

            //Draw Snake
            for (uint i = 0; i < 100;i++)
            {
                var tmp=new POS();
                tmp.x=200+i;
                tmp.y=200;
                snake.Add(tmp);
                System.Windows.Forms.Cursor.Position = new Point((int)snake[snake.Count - 1].x, (int)snake[snake.Count - 1].y);
                uint mousex = (uint)System.Windows.Forms.Cursor.Position.X;
                uint mousey = (uint)System.Windows.Forms.Cursor.Position.X;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP,mousex,mousey, 0, 0);
                Thread.Sleep(delay);
            }
            int kk = 0;
            while (true)
            {
                kk += 1;
                if (kk > 100)
                {
                    System.Windows.Forms.MessageBox.Show("You Can Stop Here!!!");
                    kk = 0;
                }
                
                //If Rat Found
                if ((snake[snake.Count - 1].x == rat.x) && (snake[snake.Count - 1].y == rat.y))
                {
                    rat.x=(uint)r.Next(200,200+size);
                    rat.y=(uint)r.Next(200,200+size);
                    Thread.Sleep(500);
                    System.Windows.Forms.MessageBox.Show("You Can Stop Here!!!");
                    System.Windows.Forms.Cursor.Position = new Point((int)rat.x, (int)rat.y);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, rat.x, rat.y, 0, 0);
                    Thread.Sleep(500);
                }
                //Move Snake
                System.Windows.Forms.Cursor.Position = new Point((int)snake[0].x, (int)snake[0].y);
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, snake[0].x, snake[0].y, 0, 0);
                if (rat.x > snake[snake.Count - 1].x)
                {
                    var tmp=new POS();
                    tmp.x=snake[snake.Count - 1].x+1;
                    tmp.y = snake[snake.Count - 1].y;
                    snake.Add(tmp);
                }
                else if (rat.y > snake[snake.Count - 1].y)
                {   
                    var tmp=new POS();
                    tmp.x = snake[snake.Count - 1].x;
                    tmp.y = snake[snake.Count - 1].y+1;
                    snake.Add(tmp);
                }    
                else if (rat.x < snake[snake.Count - 1].x)
                {   
                    var tmp=new POS();
                    tmp.x = snake[snake.Count - 1].x-1;
                    tmp.y = snake[snake.Count - 1].y;
                    snake.Add(tmp);
                }
                else if (rat.y < snake[snake.Count - 1].y)
                {
                    var tmp = new POS();
                    tmp.x = snake[snake.Count - 1].x;
                    tmp.y = snake[snake.Count - 1].y-1;
                    snake.Add(tmp);
                }
                snake.RemoveAt(0);
                Thread.Sleep(delay);
                System.Windows.Forms.Cursor.Position = new Point((int)snake[snake.Count - 1].x, (int)snake[snake.Count - 1].y);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, snake[snake.Count - 1].x, snake[snake.Count - 1].y, 0, 0);
                Thread.Sleep(delay);
                k = (k + 1) % 100;

        
            }
        }
    }
}
