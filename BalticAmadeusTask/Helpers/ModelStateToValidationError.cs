using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalticAmadeusTask.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BalticAmadeusTask.Helpers
{
    public static class ModelStateToValidationError
    {
        public static DataValidationError Convert(ModelStateDictionary modelStateDictionary)
        {
            var dataValidationError = new DataValidationError();
            modelStateDictionary.Keys.ToList().ForEach(key =>
            {
                var errorV = new AttributeValidationError { Field = key };
                modelStateDictionary[key].Errors.ToList().ForEach(x => errorV.Errors.Add(x.ErrorMessage));
                dataValidationError.AttributeValidationErrors.Add(errorV);
            });
            return dataValidationError;
        }

        public static void AddModelStateError(ModelStateDictionary modelStateDictionary, DataValidationError dataValidationError)
        {
            dataValidationError.AttributeValidationErrors?.ForEach(error =>
            {
                error.Errors.ForEach(x => { modelStateDictionary.AddModelError(error.Field, x); });
            });
        }
    }
}
