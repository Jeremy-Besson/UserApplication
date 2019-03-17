using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BalticAmadeusTask.Models
{
    public class RegisteredUser
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(1024)]
        [DisplayName("Additional Info")]
        public string AdditionalInfo { get; set; }
    }
}
