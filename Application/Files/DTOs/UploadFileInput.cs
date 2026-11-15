namespace Application.Files.DTOs
{
    // Framework-agnostic file (the controller maps IFormFile to this, so Application doesn't depend on ASP.NET)
    public class UploadFileInput
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public Stream Content { get; set; }
    }
}
