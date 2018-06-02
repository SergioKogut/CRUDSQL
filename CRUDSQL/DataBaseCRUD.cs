using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using IMenu = System.Collections.Generic.List<System.Tuple<string, GetMethod>>;
using ItemMenu = System.Tuple<string, GetMethod>;
using MenuSpace;

namespace CRUDSQL
{

    // клас підтримки роботи з базою данних
    class DataBaseCRUD
    {
        // клас основне вертикальне меню з делегатами
        private Menu Menu1;

        // вертикальне меню текстове
        private VerticalMenu MenuFind;

        // флаг виходу з програми
        private bool ExitFlag = true;

        // !ЗМІНЕНО НА КОНФІГУРАТОР З КОНФІГУРАЦІЙНОГО ФАЙЛУ
        // стічка адреси підключення до  бази данних
        // private string strCon = @"Data Source = DESKTOP-08C9EQ1\SQLEXPRESS;Initial Catalog = Example1DB; Integrated Security = True;";

            // підтягуємо з конфігураційного файлу
        private string strCon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //конструктор по замовчуванню
        public DataBaseCRUD()
        {
            // створюємо допоміжне вертикальне меню
            MenuFind = new VerticalMenu(new List<string> { "по імені", "по прізвищу", "по батькові", "по email" });
            //створюємо основне меню
            Menu1 = new Menu(1, 1, new IMenu

            {
               //додаємо делегати на основні функції роботи з базою данних
                new ItemMenu(" Додати", new GetMethod(Add)), //додати користувача
                new ItemMenu(" Видалити", new GetMethod(Delete)), // видалити користувача
                new ItemMenu(" Редагувати", new GetMethod(Update)), // редагувати користувача
                new ItemMenu(" Знайти", new GetMethod(Find)), // пошук користувача
                new ItemMenu(" Допомога", new GetMethod(Help)), // допомога
                new ItemMenu(" Вихід", new GetMethod(Exit)) // вихід
            });
            //друк таблиці
            Print();
            
        }

        // запуск програми
        public void Run()
        {
            do
            { // основний цикл
                Menu1.Show();

            } while (ExitFlag);
        }

        //друк всіх полів з бази даних
        private void Print()
        {
            var users = GetAllUsers();
            ShowUsers(users);
        }

        // додавання користувача в пункті головного меню
        private void Add()
        {
            AddUser(GetUser());
            Console.Clear();
            Print();
        }


        // видалення користувача в пункті головного меню
        private void Delete()
        {
            //викликаємо меню
            int number = MenuFind.Show(13, 2);
            string[] text = { "імя", "прізвище", " по батькові", "email" };
            string findBy = "Введіть " + text[number];
            Console.SetCursorPosition(0, 10);
            Console.Write($"{findBy}: ");
            string word = Console.ReadLine();

            var users = GetAllUsers(number, word);

            // якщо кількість знайдених кристувачів не дорівню нулю
            if (users.Count != 0)
            {
                ShowUsers(users);
                int Id;

                do
                {
                    Console.SetCursorPosition(0, 1);
                    Console.Write("Введіть id User\n для видалення: ");
                } while (!Int32.TryParse(Console.ReadLine(), out Id));

            // якщо введене Id не знайдене в виведеному списку  
                if (users.Find(x => x.Id == Id) != null)
                {
                    // видалення  по Id
                    DelUser(Id);
                    Message(0, 15, ConsoleColor.Red, "Користувача видалено!");
                }
                else
                {
                    Message(0, 15, ConsoleColor.Red, "Користувач з вказаним ID не підходить під параметри вашого пошуку!");
                }
            }
            else
            {
                // вивід  користувачів
                Message(0, 15, ConsoleColor.Red, "Користувачів з такими параметрами в БД немає!");
            }
            Console.Clear();
            //друк таблиці
            Print();
        }

        // редагування користувача в головному меню
        private void Update()
        {

            int number = MenuFind.Show(13, 3);
            string[] text = { "імя", "прізвище", " по батькові", "email" };
            string findBy = "Введіть " + text[number];
            Console.SetCursorPosition(0, 10);

            Console.Write($"{findBy}: ");
            string word = Console.ReadLine();

            var users = GetAllUsers(number, word);

            if (users.Count != 0)
            {
                ShowUsers(users);

                int Id;

                do
                {
                    Console.SetCursorPosition(0, 1);
                    Console.Write("Введіть id User\n для редагування: ");
                } while (!Int32.TryParse(Console.ReadLine(), out Id));


                if (users.Find(x => x.Id == Id) != null)
                {
                    // видалення 
                    UpdateUser(Id, GetUser());
                    Message(0, 15, ConsoleColor.Red, "Користувача відредаговано!");
                }
                else
                {
                    // якщо введене Id користувача не співпадає з списком
                    Message(0, 15, ConsoleColor.Red, "Користувач з вказаним ID не підходить під параметри вашого пошуку!");
                }
            }
            else
            {
                // вивід  користувачів

                Message(0, 15, ConsoleColor.Red, "Користувачів з такими параметрами в БД немає!");
            }
            Console.Clear();
            //друк таблиці
            Print();
        }

        // пошук користувача в головному меню
        private void Find()
        {
            int number = MenuFind.Show(13, 4);
            string[] text = { "імя", "прізвище", " по батькові", "email" };
            string findBy = "Введіть " + text[number];
            Console.SetCursorPosition(0, 10);

            Console.Write($"{findBy}: ");
            string word = Console.ReadLine();

            var users = GetAllUsers(number, word);

            if (users.Count != 0)
            {
                ShowUsers(users);
            }
            else
            {
                Message(0, 15, ConsoleColor.Red, "Користувачів з такими параметрами в БД немає!");
            }
            Console.ReadKey();
            Console.Clear();
            //друк таблиці
            Print();
        }

        //вихід з програми
        private void Help()
        {
            Message(0, 15, ConsoleColor.DarkYellow, "Ми вам допоможемо,обовязково!");
        }

        //вихід з програми
        private void Exit()
        {
            Console.SetCursorPosition(0, 15);
            Message(0, 15, ConsoleColor.DarkYellow, "Дякую за користуванням програмою!");
            ExitFlag = false;
        }

        // показ списку користувачів
        void ShowUsers(List<UserViewModel> users)
        {
            int i = 1;
            Console.Clear();
            Console.SetCursorPosition(27, 0);
            Console.WriteLine("№    Id      Full name                       Email");
            foreach (var user in users)
            {
                Console.SetCursorPosition(27, i);
                Console.WriteLine($"{i} {user}");
                i++;
            }

        }

        // створення користувача
        UserCreateModel GetUser()
        {
            Console.SetCursorPosition(0, 8);
            UserCreateModel user = new UserCreateModel();
            Console.Write("Прізвище: ");
            user.LastName = Console.ReadLine();
            Console.Write("Ім'я: ");
            user.FirstName = Console.ReadLine();
            Console.Write("По батькові: ");
            user.Surname = Console.ReadLine();
            Console.Write("Електронна адреса: ");
            user.Email = Console.ReadLine();
            Console.Write("Пароль: ");
            user.Password = Console.ReadLine();

            return user;

        }



        // ! додаємого нового User в таблицю БД 
        private void AddUser(UserCreateModel user)
        {

            string strQuery = "INSERT INTO Users " +
               "(FirstName, LastName, Surname, Password, PasswordSalt, Email) " +
                $"VALUES('{user.FirstName}','{user.LastName}','{user.Surname}', " +
                $"'{user.Password}','{user.Password}','{user.Email}'); ";

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                {
                    conn.Open();
                    int row = cmd.ExecuteNonQuery();
                    if (row == 0)
                    {
                        Message(0, 15, ConsoleColor.Red, "---Помилка додавання користувача!---");
                    }

                }

            }

        }

        // !SQL видаляємо  User з БД  по Id
        private void DelUser(int UserId)
        {
            string strQuery = $"DELETE FROM Users WHERE Id='{UserId}';";
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                {
                    conn.Open();
                    int row = cmd.ExecuteNonQuery();
                    if (row == 0)
                    {
                        Message(0, 15, ConsoleColor.Red, "---Помилка видалення користувача!---");
                    }

                }

            }
        }

        // !SQL редагуємо  User з БД  по Id
        private void UpdateUser(int UserId, UserCreateModel user)
        {

            string strQuery = $"UPDATE Users SET FirstName='{user.FirstName}', LastName='{user.LastName}', Surname='{user.Surname}', " +
                $"Password='{user.Password}', PasswordSalt='{user.Password}', Email='{user.Email}' WHERE Id='{UserId}';";
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                {
                    conn.Open();
                    int row = cmd.ExecuteNonQuery();
                    if (row == 0)
                    {
                        Message(0, 15, ConsoleColor.Red, "---Помилка редагування користувача!---");
                    }

                }

            }
        }


       

        

        // видає список користувачів по певному полю або всіх
        private List<UserViewModel> GetAllUsers(int findBy = -1, string selectBy = "")
        {

            List<UserViewModel> users = new List<UserViewModel>();
            string strQuery = "";
            switch (findBy)
            {
                case -1:
                    strQuery = "SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U";
                    break;

                case 0:
                    strQuery = $"SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U WHERE FirstName = '{selectBy}'";
                    break;
                case 1:
                    strQuery = $"SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U WHERE LastName = '{selectBy}'";
                    break;
                case 2:
                    strQuery = $"SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U WHERE Surname = '{selectBy}'";
                    break;
                case 3:
                    strQuery = $"SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U WHERE Email = '{selectBy}'";
                    break;


                default:
                    break;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    using (SqlCommand cmd = new SqlCommand(strQuery, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                UserViewModel user = new UserViewModel
                                {
                                    Id = int.Parse(reader["UserId"].ToString()),
                                    Email = reader["Email"].ToString(),
                                    FullName =
                                         reader["LastName"].ToString() + " " +
                                         reader["FirstName"].ToString() + " " +
                                         reader["Surname"].ToString()
                                };
                                users.Add(user);

                            }

                        }

                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


            return users;
        }


        // вивід повідомлення на екран
        private void Message(int x = 0, int y = 0, ConsoleColor color = ConsoleColor.Green, string text = "")
        {
            ConsoleColor ExistingColor = Console.ForegroundColor;
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ReadKey();
            Console.SetCursorPosition(x, y);
            Console.WriteLine(new string(' ', text.Length));
            Console.ForegroundColor = ExistingColor;
        }



    }
}
