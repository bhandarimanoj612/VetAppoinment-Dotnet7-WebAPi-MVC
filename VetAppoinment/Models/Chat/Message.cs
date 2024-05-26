namespace VetAppoinment.Models.Chat
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; } // Changed from SenderId to SenderUsername
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
