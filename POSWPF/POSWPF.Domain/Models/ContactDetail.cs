using System.ComponentModel.DataAnnotations.Schema;

namespace ECR.Domain.Models {
    [Table(nameof(ContactDetail))]
    public sealed class ContactDetail {
        public int Id { get; set; }

        /// <summary>
        /// detail type like: cellphone number, email, telephone, fax etc
        /// </summary>
        public ContactType Type { get; set; } = ContactType.Mobile;
        /// <summary>
        /// the actual contact detail
        /// </summary>
        public string Value { get; set; } = null!;
    }
}
