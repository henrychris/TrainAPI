namespace TrainAPI.Domain.Entities
{
    public class Train
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}