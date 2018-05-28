using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IMenu = System.Collections.Generic.List<System.Tuple<string, GetMethod>>;
using ItemMenu = System.Tuple<string, GetMethod>;
using MenuSpace;

namespace CRUDSQL
{

    // клас підтримки роботи з базою данних
    class DataBaseCRUD
    {
        // основне меню
        private Menu Menu1;

        // горизонтальне меню
        private HorizontalMenu MenuSize;

        // вертикальне меню
        private VerticalMenu MenuFind;

        // флаг виходу з програми
        private bool ExitFlag = true;

        // стічка бази данних
        private string strCon = @"Data Source = DESKTOP-08C9EQ1\SQLEXPRESS;Initial Catalog = Example1DB; Integrated Security = True;";

        //конструктор по замовчуванню
        public DataBaseCRUD()
        {

            //створюємо основне меню
            Menu1 = new Menu(1, 1, new IMenu

            {
                new ItemMenu(" Друк всіх", new GetMethod(Print)),
                new ItemMenu(" Додати", new GetMethod(Add)),
                new ItemMenu(" Видалити", new GetMethod(Print)),
                new ItemMenu(" Змінити", new GetMethod(Print)),
                new ItemMenu(" Допомога", new GetMethod(Help)),
                new ItemMenu(" Вихід", new GetMethod(Exit))
            });

            //MenuSize = new HorizontalMenu(10, 12, new List<string> { "", "3", "4", "5", "6" });
            MenuFind = new VerticalMenu(10, 14, new List<string> { "по імені", "по прізвищу", "по батькові", "по email"});
        }

        private List<UserViewModel> GetAllUsers(string select = "")
        {

            List<UserViewModel> users = new List<UserViewModel>();


            //  string strCon1 = "Data Source=10.7.0.5;Initial Catalog=StudStep5;User ID=test;Password=123456qwerty";
            //  string strCon = @"Data Source=DESKTOP-08C9EQ1$\SQLEXPRESS;Initial Catalog=Sergio;Integrated Security=True";
            //string strCon1 = @"Data Source =.; Initial Catalog = Example1DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
           // string strCon = @"Data Source = DESKTOP-08C9EQ1\SQLEXPRESS;Initial Catalog = Example1DB; Integrated Security = True;";
            string strQuery = "SELECT U.Id as UserId,U.LastName,U.FirstName,Surname,Email FROM Users as U";
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





        // запуск програми
        public void Run()
        {
            do
            {
                Menu1.Show();
            } while (ExitFlag);
        }

        //друк всіх полів 
        private void Print()
        {
            var users = GetAllUsers();
            int i = 1;

            Console.SetCursorPosition(25, 0);
            Console.WriteLine("№      Id      Full name                       Email");
            foreach (var user in users)
            {
                Console.SetCursorPosition(25, i);

                Console.WriteLine($"{i} {user}");
                i++;
            }
            
        }

        private void Add()
        {
            Console.SetCursorPosition(0, 15);
            
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
            AddUser(user);
            
            Console.Clear();
        }

        private void AddUser(UserCreateModel user)
        {

           // string strCon = "Data Source=10.7.0.5;Initial Catalog=StudStep5;User ID=test;Password=123456qwerty";
            string strQuery = "INSERT INTO Users (FirstName, LastName, Surname, Password, PasswordSalt, Email) " +
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
                        Console.WriteLine("---Помилка додавання користувача!---");
                    }

                }

            }

        }


        //вихід з програми
        private void Help()
        {
            Console.SetCursorPosition(0, 15);
            string text = "Ми вам допоможемо!";
            Console.WriteLine(text);
            Console.ReadKey();
            Console.Clear();
        }

        //вихід з програми
        private void Exit()
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("Вихід\n Дякую за користуванням програмою!");
            ExitFlag = false;
        }

        
    }
}
