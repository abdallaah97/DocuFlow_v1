namespace Application.Requests.DTOs
{
    public class SaveRequestInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int ClientId { get; set; }
        public List<SaveRequestItemInput> Items { get; set; }
    }

    public class SaveRequestItemInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
