using System;
using System.Collections.Generic;

namespace Quan_Li_Chi_Tieu.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public DateOnly MonthYear { get; set; }

    public decimal LimitAmount { get; set; }

    public int? AlertThreshold { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
