namespace ECR.Domain.Models {
    public sealed class Name {
        public string First { get; set; } = null!;
        public string? Middle { get; set; } = null;
        public string? Last { get; set; } = null;

        public override string ToString() => First + (string.IsNullOrWhiteSpace(Middle) ? "" : " " + Middle) + (string.IsNullOrWhiteSpace(Last)
            ? "" : " " + Last);
    }
}
