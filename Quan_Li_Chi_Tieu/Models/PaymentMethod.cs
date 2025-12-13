using System;
using System.Collections.Generic;

namespace Quan_Li_Chi_Tieu.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public int UserId { get; set; }

    public string MethodName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
