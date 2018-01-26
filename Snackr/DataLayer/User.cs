namespace Snackr.DataLayer
{
    public class User
    {
        public string _Email { get; set; }
        public string _Name { get; set; }
        public string _Permission { get; set; }
        public int _TakoKoinCount { get; set; }
        
        public User() { }

        public User(string email, string name, string permission, int takokoincount)
        {
            _Email = email;
            _Name = name;
            _Permission = permission;
            _TakoKoinCount = takokoincount;    // keep 0 until tako koin implemented
        }
      

    }
}