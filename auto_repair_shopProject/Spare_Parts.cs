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
    
    public partial class Spare_Parts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Spare_Parts()
        {
            this.History_Orders = new HashSet<History_Orders>();
        }
    
        public int id_part { get; set; }
        public Nullable<int> id_service { get; set; }
        public string name_parts { get; set; }
        public byte[] photo { get; set; }
        public string description { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> presence { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History_Orders> History_Orders { get; set; }
        public virtual Services Services { get; set; }
    }
}