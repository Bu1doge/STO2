//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ООО_Поломка_STO2.EFData
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientView
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Email { get; set; }
        public System.DateTime RegisatrationDate { get; set; }
        public Nullable<System.DateTime> LastVisit { get; set; }
        public Nullable<int> VisitAmount { get; set; }
        public string Tags { get; set; }
        public string PhoneNumber { get; set; }
    }
}