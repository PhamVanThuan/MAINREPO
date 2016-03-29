namespace Automation.DataModels
{
    public abstract class Record 
    {
        public Record(string schema = "dbo", string db = "2am")
        {
            this.Schema = schema;
            this.Table = this.GetType().Name;
            this.DB = db;
        }
        public string Schema { get; set; }
        public string DB { get; set; }
        public int Key { get; set; }
        public string Table { get; set; }
    }
}
