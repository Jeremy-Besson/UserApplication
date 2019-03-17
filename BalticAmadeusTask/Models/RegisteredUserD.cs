using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BalticAmadeusTask.Models
{
    [Table("RegisteredUser")]
    public class RegisteredUserD
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; } = null;
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(1024)]
        [DisplayName("Additional Info")]
        public string AdditionalInfo { get; set; }
    }
}
