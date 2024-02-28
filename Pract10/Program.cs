using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pract10
{
    delegate void FinishGameDelegate(object sender, EventArgs eventArgs);


    class FinishCathcherEventArgs : EventArgs
    {
        public FinishCathcherEventArgs(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x, y;
    }

    
    class FinishTrigger
    {
        public event FinishGameDelegate FinishGame;
        
        public void CheckFinishPosition(object sender, EventArgs eventArgs)
        {
            FinishCathcherEventArgs args = (FinishCathcherEventArgs)(eventArgs);
            if(args == null) 
                throw new ArgumentException("Argument of event failed");
            if ((args.x==9 && args.y==16) || (args.x==46 && args.y==17))
            {
                if (FinishGame == null)
                    throw new Exception("Event have not receivers");
                FinishGame(sender, eventArgs);
            }
        }
    }
    sealed class FinishCathcher
    {
        static protected bool gameFinished = false;
        static public bool IsGameFinished()
        {
            return gameFinished;
        }

        public void CathFinishEvent(object sender, EventArgs eventArgs)
        {
            gameFinished=true;

            Console.Clear();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вы вышли из лабиринта!");
            Console.ResetColor();
            Console.WriteLine($"Ваша позиция:\nX:{((FinishCathcherEventArgs)(eventArgs)).x}\tY:{((FinishCathcherEventArgs)(eventArgs)).y}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Program Action = new Program();


            FinishTrigger finishTrigger = new FinishTrigger();
            FinishCathcher finishCathcher = new FinishCathcher();
            finishTrigger.FinishGame += new FinishGameDelegate(finishCathcher.CathFinishEvent);




            
            Console.CursorVisible=false;
            //Допуски на перемещение
            bool up = true, left = true, right = true, down = true;
            int x = 15, y = 15;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"X:{x}\tY:{y}");
            Console.ResetColor();
            Action.Draw(x, y, ref up, ref left, ref right, ref down);
            Console.SetCursorPosition(x, y);
            Console.Write("*");
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow: if (up) y--; break;
                    case ConsoleKey.DownArrow: if (down) y++; break;
                    case ConsoleKey.LeftArrow: if (left) x--; break;
                    case ConsoleKey.RightArrow: if (right) x++; break;
                    case ConsoleKey.Escape: return;
                    default: break;
                }
                up = true; left = true; right = true; down = true;
                finishTrigger.CheckFinishPosition(finishTrigger, new FinishCathcherEventArgs(x,y));
                if (FinishCathcher.IsGameFinished())
                    break;
                //Рисуем карту:
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"X:{x}\tY:{y}");
                Console.ResetColor();
                Action.Draw(x, y, ref up, ref left, ref right, ref down);
                //Запрет на преодоление границ ком.строки:
                if (x > 78) right = false;
                if (x < 1) left = false;
                if (y < 2) up = false;
                if (y > 23) down = false;
                //Выводим точку
                Console.SetCursorPosition(x, y);
                Console.Write("*");
            }
        }
        //Горизонтальная
        void DrawHLine(int x, int y, int from, int to, int yLine,
                       ref bool up, ref bool down)
        {
            for (int i = from; i <= to; i++)
            {
                if ((y - yLine == -1) && (x >= from) && (x <= to)) down = false;
                if ((y - yLine == 1) && (x >= from) && (x <= to)) up = false;
                Console.SetCursorPosition(i, yLine);
                Console.Write("#");
            }
        }
        //Вертикальная
        void DrawVLine(int x, int y, int from, int to, int xLine,
                       ref bool left, ref bool right)
        {
            for (int i = from; i <= to; i++)
            {
                if ((x - xLine == -1) && (y >= from) && (y <= to)) right = false;
                if ((x - xLine == 1) && (y >= from) && (y <= to)) left = false;
                Console.SetCursorPosition(xLine, i);
                Console.Write("#");
            }
        }
        //Карта
        void Draw(int x, int y, ref bool up, ref bool left, ref bool right, ref bool down)
        {
            DrawHLine(x, y, 10, 30, 5, ref up, ref down);
            DrawVLine(x, y, 5, 10, 30, ref left, ref right);
            DrawHLine(x, y, 30, 43, 10, ref up, ref down);
            DrawVLine(x, y, 5, 10, 40, ref left, ref right);
            DrawHLine(x, y, 40, 45, 5, ref up, ref down);
            DrawVLine(x, y, 5, 16, 45, ref left, ref right);
            DrawVLine(x, y, 18, 20, 45, ref left, ref right);
            DrawHLine(x, y, 10, 45, 20, ref up, ref down);
            DrawVLine(x, y, 5, 15, 10, ref left, ref right);
            DrawVLine(x, y, 17, 20, 10, ref left, ref right);
        }
    }
}
