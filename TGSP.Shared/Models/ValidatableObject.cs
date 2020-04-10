using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TGSP.Shared.Models
{
    /// <summary>
    /// This class can be used as a based to validate object
    /// </summary>
    public class ValidatableObject
    {
        /// <summary>
        /// This method will validate the object creating a context based on this object
        /// </summary>
        /// <remarks>This method will only perform static analysis!</remarks>
        /// <returns>A list with validation result</returns>
        public IEnumerable<ValidationResult> Validate()
        {
            var validationContext = new ValidationContext(this);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext,result, true);
            return result;
        }

        /// <summary>
        /// This method will validate the object creating a context based on this object
        /// </summary>
        /// <remarks>This method will only perform static analysis!</remarks>
        /// <returns>Will return true for a valid object</returns>
        public bool IsValid() => Validate().Count() == 0;
    }
}