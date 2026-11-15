using Application.Files;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Files
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _rootPath;
        public LocalFileStorage(IConfiguration configuration)
        {
            _rootPath = configuration["Attachments:Path"];
            if (string.IsNullOrWhiteSpace(_rootPath))
            {
                throw new Exception("Attachments:Path is not configured in appsettings");
            }
        }

        public async Task<string> Save(Stream content, string folder, string extension)
        {
            var directory = Path.Combine(_rootPath, folder);
            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directory, fileName);

            using (var stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await content.CopyToAsync(stream);
            }

            return Path.Combine(folder, fileName).Replace('\\', '/');
        }

        public void Delete(string relativePath)
        {
            var fullPath = Path.Combine(_rootPath, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
