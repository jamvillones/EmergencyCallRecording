namespace ECR.Domain.Models {
    public sealed class Address {
        public HouseNumber? HouseNumber { get; set; } = null;
        public Street? Street { get; set; } = null;
        public Area? Area { get; set; } = null;
        public Barangay Barangay { get; set; } = null!;
    }
}
