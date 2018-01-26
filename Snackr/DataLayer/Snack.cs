namespace Snackr.DataLayer
{
    public class Snack
    {
        public string _snack_brand { get; set; }
        public string _snack_name { get; set; }
        public int _snack_count { get; set; }
        
        public Snack() {}
        
        public Snack(string snack_brand, string snack_name, int request_count)
        {
            _snack_brand = snack_brand;
            _snack_name = snack_name;
            _snack_count = request_count;
        }

    }
}