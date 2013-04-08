namespace Common
{
    /// <summary>
    /// Represents a country that mail can be sent to.
    /// </summary>
    public class Country: DataObject
    {
       
        public Country(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
