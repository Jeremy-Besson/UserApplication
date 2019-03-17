using System.Collections.Generic;

namespace BalticAmadeusTask.Models
{
    public class AttributeValidationError
    {
        public string Field { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class DataValidationError
    {
        public List<AttributeValidationError> AttributeValidationErrors { get; set; } = new List<AttributeValidationError>();
    }
}
