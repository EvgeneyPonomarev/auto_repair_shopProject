//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace auto_repair_shopProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class History_Orders
    {
        public int id_record { get; set; }
        public Nullable<int> id_client { get; set; }
        public Nullable<int> id_car { get; set; }
        public Nullable<int> id_services { get; set; }
        public Nullable<int> id_part { get; set; }
        public Nullable<int> id_employer { get; set; }
    
        public virtual Cars Cars { get; set; }
        public virtual Clients Clients { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual Spare_Parts Spare_Parts { get; set; }
        public virtual Services Services { get; set; }
    }
}
