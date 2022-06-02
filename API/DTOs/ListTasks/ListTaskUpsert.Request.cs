namespace API.DTOs.ListTasks
{
    public class ListTaskUpsertRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
    }
}
