using System.ComponentModel.DataAnnotations.Schema;

namespace ECR.Domain.Models {
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
        public int PriorityLevel { get; set; } = 0;
        public Caller Call { get; set; } = null!;
        /// <summary>
        /// place where the incident took place
        /// </summary>
        public string IncidentLocation { get; set; } = null!;
        /// <summary>
        /// the distinguishing landmark for operations
        /// </summary>
        public string? Landmark { get; set; } = null;
        /// <summary>
        /// to transfer to
        /// </summary>
        public Agency? Agency { get; set; }

        public List<Audio> Audios { get; set; } = new();
    }

    [Table(nameof(Audio))]
    public sealed class Audio {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateRecorded { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}
