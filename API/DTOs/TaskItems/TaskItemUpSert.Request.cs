namespace API.DTOs.TaskItems
{
    public class TaskItemUpSertRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int ListTaskId { get; set; }
    }
}
