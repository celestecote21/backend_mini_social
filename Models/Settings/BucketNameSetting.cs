
namespace netart.Models
{
    public class BucketNameSetting : IBucketNameSetting{

        public string bucketName { get; set; }
    }
    public interface IBucketNameSetting {
        public string bucketName { get; set; }
    }
    
}