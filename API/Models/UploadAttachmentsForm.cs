namespace API.Models
{
    public class UploadAttachmentsForm
    {
        public string Token { get; set; }
        public int ItemId { get; set; }
        public IFormFile File { get; set; }
    }
}
