using System;
using System.Collections.Generic;

namespace Quan_Li_Chi_Tieu.Models;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public int TransactionId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public int? FileSize { get; set; }

    public string? MimeType { get; set; }

    public DateTime? UploadedDate { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
