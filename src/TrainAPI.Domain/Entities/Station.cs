namespace TrainAPI.Domain.Entities
{
    public class Station
    {
        // todo: add column length for DB properties
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Code { get; set; } // todo: don't allow duplicate codes.
    }
}