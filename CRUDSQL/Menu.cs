using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuSpace
{
    /*
      Клас  горизонтального меню   
    */

    class HorizontalMenu
    {
        private List<string> MenuItem;
        private int ActiveItem = 0;
        private ConsoleKeyInfo key;
        private int PosXmenu = Console.CursorLeft;
        private int PosYmenu = Console.CursorTop;

        public HorizontalMenu(List<string> temp)
        {
            MenuItem = temp;

        }

        public HorizontalMenu(int x, int y, List<string> temp)
        {
            MenuItem = temp;
            PosXmenu = x;
            PosYmenu = y;
        }

        public void SetY(int y)
        {
            PosYmenu = y;

        }

        private void SetColorText(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public string Show()
        {
            do
            {
                //друк меню

                for (int i = 0; i < MenuItem.Count; i++)
                {
                    if (i == ActiveItem)
                    {
                        SetColorText(PosXmenu + i * 3, PosYmenu, MenuItem[i], ConsoleColor.Yellow);
                    }
                    else
                    {
                        SetColorText(PosXmenu + i * 3, PosYmenu, MenuItem[i], ConsoleColor.Green);
                    }
                }
                //перевірка нажаття клавіш
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow && ActiveItem < MenuItem.Count - 1)
                    ActiveItem++;
                if (key.Key == ConsoleKey.LeftArrow && ActiveItem > 0)
                    ActiveItem--;


            }//виході якщо нажати Ентер
            while (key.Key != ConsoleKey.Enter);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            return MenuItem[ActiveItem]; // вертаємо значення горизонтального меню

        }
    }

    class VerticalMenu
    {
        private List<string> MenuItem;
        private int ActiveItem = 0;
        private ConsoleKeyInfo key;
        private int PosXmenu = Console.CursorLeft;
        private int PosYmenu = Console.CursorTop;

        public VerticalMenu(List<string> temp)
        {
            MenuItem = temp;

        }

        public VerticalMenu(int x, int y, List<string> temp)
        {
            MenuItem = temp;
            PosXmenu = x;
            PosYmenu = y;
        }

        private void SetColorText(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public int Show(int x=0, int y=0)
        {
            PosXmenu = x; PosYmenu = y;
            do
            {
                //друк меню

                for (int i = 0; i < MenuItem.Count; i++)
                {
                    if (i == ActiveItem)
                    {
                        SetColorText(PosXmenu, PosYmenu + i, MenuItem[i], ConsoleColor.Yellow);
                    }
                    else
                    {
                        SetColorText(PosXmenu, PosYmenu + i, MenuItem[i], ConsoleColor.Green);
                    }
                }
                //перевірка нажаття клавіш
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow && ActiveItem < MenuItem.Count - 1)
                    ActiveItem++;
                if (key.Key == ConsoleKey.UpArrow && ActiveItem > 0)
                    ActiveItem--;

            }//виході якщо нажати Ентер
            while (key.Key != ConsoleKey.Enter);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            return ActiveItem; // вертаємо значення горизонтального меню

        }


        public void Clear()
        {
            for (int i = 0; i < MenuItem.Capacity; i++)
            {
                Console.SetCursorPosition(PosXmenu, PosYmenu + i);
                Console.WriteLine(new string(' ', MenuItem.Max().Length));
            }

        }
    }


    /*
      Клас  вертикального меню  з делегатами 
    */


    class Menu
    {
        // активний пункт меню
        private int ActiveItem = 0;

        // ліва позиція меню по Х 
        private int PosXmenu = 10;

        // ліва позиція меню по Y
        private int PosYmenu = 10;
        
        // делегат 
        private GetMethod Method;

        //список меню
        private List<Tuple<string, GetMethod>> MenuItem;

        // колір активного меню
        private ConsoleColor ActiveMenuColor = ConsoleColor.White;

        // колір пасивного меню
        private ConsoleColor PassiveMenuColor = ConsoleColor.Green;

        //нажаття клавіши
        private ConsoleKeyInfo key;

        public Menu() { }
        public Menu(int x, int y, List<Tuple<string, GetMethod>> temp)
        {
            PosXmenu = x;
            PosYmenu = y;
            MenuItem = temp;
        }
        private void SetColorText(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        public void Show()
        {
            do
            {
                //друк меню
                for (int i = 0; i < MenuItem.Count; i++)
                {
                    if (i == ActiveItem)
                    {
                        SetColorText(PosXmenu, PosYmenu + i, MenuItem[i].Item1, ActiveMenuColor);
                    }
                    else
                    {
                        SetColorText(PosXmenu, PosYmenu + i, MenuItem[i].Item1, PassiveMenuColor);
                    }
                }
                SetColorText(PosXmenu, PosYmenu - 1, "  МЕНЮ", ConsoleColor.Green);

                //перевірка нажаття клавіш
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow && ActiveItem < MenuItem.Count - 1)
                {
                    ActiveItem++;
                }
                if (key.Key == ConsoleKey.UpArrow && ActiveItem > 0)
                {
                    ActiveItem--;
                }

            }//вихі якщо нажади Ентер
            while (key.Key != ConsoleKey.Enter);
            // Console.Clear();
            Method = MenuItem[ActiveItem].Item2;
            Method.Invoke();
        }


        // добавлення нового пункту меню ззовні класу
        public void Add(Tuple<string, GetMethod> item)
        {
            MenuItem.Add(item);
        }

    }
}