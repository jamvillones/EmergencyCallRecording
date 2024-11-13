using System.ComponentModel.DataAnnotations.Schema;

namespace ECR.Domain.Models {
    public enum PriorityLevel { One, Two, Three, Four, Five, Six }

    [Table(nameof(Record))]
    public sealed class Record {
        public int Id { get; set; }
        /// <summary>
        /// optional summary of the incident
        /// </summary>
        public string? Summary { get; set; } = null;
        /// <summary>
        /// the type of call, be it emergency, prank etc.e
        /// </summary>
        public string? CallType { get; set; } = null;
        public string Details { get; set; } = null!;
        /// <summary>
        /// levels from 1 - 3
        /// </summary>
        public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.One;
        public Caller Call { get; set; } = null!;
        /// <summary>
        /// place where the incident took place
        /// </summary>
        public string IncidentLocation { get; set; } = null!;
        /// <summary>
        /// the distinguishing landmark for operations
        /// </summary>
        public DateTime DateTimeOfReport { get; set; } = DateTime.Now;
        /// <summary>
        /// to transfer to
        /// </summary>
        public Agency? Agency { get; set; } = null;

        public string CallReceiver { get; set; } = null!;

        /// <summary>
        /// the one who recorded the record
        /// </summary>
        public Login? Login { get; set; } = null;

        public List<Audio> Audios { get; set; } = [];
    }

    [Table(nameof(Audio))]
    public sealed class Audio {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateRecorded { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public Record Record { get; set; } = null!;
    }
}
