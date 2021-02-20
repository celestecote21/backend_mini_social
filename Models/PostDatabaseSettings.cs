namespace netart.Models
{
    public class PostDatabaseSetting: IPostDatabaseSetting
    {
        public string PostCollectionName
        {
            set;
            get;
        }
        public string ConnectionString
        {
            set;
            get;
        }
        public string DatabaseName
        {
            set;
            get;
        }
    }
    public interface IPostDatabaseSetting
    {
        string PostCollectionName
        {
            set;
            get;
        }
        string ConnectionString
        {
            set;
            get;
        }
        string DatabaseName
        {
            set;
            get;
        }
    }
}