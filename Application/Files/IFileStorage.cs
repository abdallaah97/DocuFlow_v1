namespace Application.Files
{
    public interface IFileStorage
    {
        public Task<string> Save(Stream content, string folder, string extension);
        public void Delete(string relativePath);
    }
}
