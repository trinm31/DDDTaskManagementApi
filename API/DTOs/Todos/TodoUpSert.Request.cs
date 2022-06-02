namespace API.DTOs.Todos
{
    public class TodoUpSertRequest
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TaskId { get; set; }
    }
}
