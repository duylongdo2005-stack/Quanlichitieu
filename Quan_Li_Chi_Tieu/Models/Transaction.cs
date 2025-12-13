using System;
using System.Collections.Generic;

namespace Quan_Li_Chi_Tieu.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public DateOnly TransactionDate { get; set; }

    public string? Description { get; set; }

    public bool? IsRecurring { get; set; }

    public string? RecurrencePattern { get; set; }

    public DateOnly? NextOccurrenceDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Category Category { get; set; } = null!;

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
