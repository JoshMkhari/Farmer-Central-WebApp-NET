using System.ComponentModel.DataAnnotations.Schema;

namespace ST1109348.Models
{
    public class ImageModel
    {
        //TO Store in database
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string UserId { get; set; }
        
    }
}