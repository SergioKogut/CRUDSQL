using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using IMenu = System.Collections.Generic.List<System.Tuple<string, GetMethod>>;
using ItemMenu = System.Tuple<string, GetMethod>;
using MenuSpace;

namespace CRUDSQL
{
    class DataBaseCRUD
    {
        private Menu Menu1;// меню
        private HorizontalMenu MenuSize;// горизонтальне меню
        private VerticalMenu MenuColor; // вертикальне меню

        private bool ExitFlag = true; // флаг выхода из програмы

        public DataBaseCRUD()
        {
            Menu1 = new Menu(1, 1, new IMenu

            {
                new ItemMenu(" Друк всіх", new GetMethod(Print)),
                new ItemMenu(" Додати", new GetMethod(Print)),
                new ItemMenu(" Видалити", new GetMethod(Print)),
                new ItemMenu(" Змінити", new GetMethod(Print)),
                new ItemMenu(" Допомога", new GetMethod(Help)),
                new ItemMenu(" Вихід", new GetMethod(Exit))
            });

        }

        private List<UserViewModel> GetAllUsers(string select = "")
        {

            List<UserViewModel> users = new List<UserViewModel>();

            
          //  string strCon1 = "Data Source=10.7.0.5;Initial Catalog=StudStep5;User ID=test;Password=123456qwerty";
          //  string strCon = @"Data Source=DESKTOP-08C9EQ1$\SQLEXPRESS;Initial Catalog=Sergio;Integrated Security=True";
            string strCon = @"Data Source = DESKTOP - 08C9EQ1\SQLEXPRESS; Initial Catalog = Example1DB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

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

        private void Print()
        {
            var users = GetAllUsers();
            int i = 1;
            foreach (var user in users)
            {
                Console.WriteLine($"{i} {user}");
                i++;
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
            Console.WriteLine("Вихід\n Дякую за користуванням програмою!");
            ExitFlag = false;
        }

        //MenuSize = new HorizontalMenu(10, 12, new List<string> { "2", "3", "4", "5", "6" });
        //MenuColor = new VerticalMenu(10, 14, new List<string> { "Синий", "Зеленый", "Бирюзовый", "Красный", "Розовый", "Желтый", "Белый" });
    }
}
