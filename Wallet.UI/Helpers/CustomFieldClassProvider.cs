using System;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;

namespace Wallet.UI.Helpers
{
    public class CustomFieldClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            if (editContext is null)
            {
                throw new ArgumentNullException(nameof(editContext));
            }

            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if (fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName)?.PropertyType == typeof(bool))
            {
                return string.Empty;
            }

            if (editContext.IsModified(fieldIdentifier))
            {
                return isValid ? "modified is-valid" : "modified is-invalid";
            }

            return isValid ? string.Empty : "is-invalid";
        }
    }
}