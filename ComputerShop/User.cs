//  2006-2008 (c) Viva64.com Team
//  2008-2020 (c) OOO "Program Verification Systems"
//  2020-2022 (c) PVS-Studio LLC

namespace ComputerShop
{
    class User
    {
        private int id;
        private string login;
        private string password;
        private int role;
        private string name;
        private string surname;
        private string email;

        internal int Id { get => id; set => id = value; }
        internal string Login { get => login; set => login = value; }
        internal string Password { get => password; set => password = value; }
        internal int Role { get => role; set => role = value; }
        internal string Name { get => name; set => name = value; }
        internal string Surname { get => surname; set => surname = value; }
        internal string Email { get => email; set => email = value; }
    }
}
