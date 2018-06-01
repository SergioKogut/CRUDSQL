using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDSQL
{
    class UserViewModel: IEquatable<UserViewModel>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            UserViewModel objAsUserViewModel = obj as UserViewModel;
            if (objAsUserViewModel == null) return false;
            else return Equals(objAsUserViewModel);
        }
        public override int GetHashCode()
        {
            return Id;
        }
        public bool Equals(UserViewModel other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
        public override string ToString()
        {
            return $"\t{Id}\t{FullName}\t\t{Email}";
        }

    }


    class UserCreateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{Id}\t\t{Email}";
        }

    }
}
