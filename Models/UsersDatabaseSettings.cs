namespace netart.Models
{
    public class UserDatabaseSetting: IUserDatabaseSetting
    {
        public string UserCollectionName
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
    public interface IUserDatabaseSetting
    {
        string UserCollectionName
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