namespace POSWPF.Domain.Models {
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
        /// to transfer to
        /// </summary>
        public Agency? Agency { get; set; }
        /// <summary>
        /// place where the incident took place
        /// </summary>
        public Address IncidentLocationk { get; set; } = null!;
        /// <summary>
        /// the distinguishing landmark for operations
        /// </summary>
        public string? Landmark { get; set; } = null;

        public List<Audio> Audios { get; set; } = new();
    }

    public sealed class Audio {
        public int Id { get; set; }
        public byte[] Clip { get; set; } = [];
    }
}
