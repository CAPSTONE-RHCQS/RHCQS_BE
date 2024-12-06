using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RHCQS_BusinessObjects.AppConstant;

namespace RHCQS_BusinessObject.Helper
{
    public class NotEmptyOrWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(ErrorMessage ?? "Giá trị không được để trống hoặc chỉ chứa khoảng trắng.");
            }

            return ValidationResult.Success!;
        }
    }
}
