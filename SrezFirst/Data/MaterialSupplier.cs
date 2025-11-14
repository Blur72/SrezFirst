using System;
using System.Collections.Generic;

namespace SrezFirst.Data;

public partial class MaterialSupplier
{
    public int MaterialId { get; set; }

    public int SupplierId { get; set; }

    public int? Empty { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
