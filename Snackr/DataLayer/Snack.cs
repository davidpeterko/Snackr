namespace Snackr
{
    public class Snack
    {
        public string snack_brand { get; set; }
        public string snack_name { get; set; }
        public int snack_count { get; set; }
        
        public Snack() {}
        
        public Snack(string b, string n, int c)
        {
            snack_brand = b;
            snack_name = n;
            snack_count = c;
        }

    }
}