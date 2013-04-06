namespace Common
{
    /// <summary>
    /// Represents a country that mail can be sent to
    /// </summary>
    public class Country
    {
       
        public Country(string name)
        {
            this.name = name;
        }

        private string name;
        public string Name 
        {
            get { return name; }
        }
    }
}
