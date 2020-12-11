using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public interface IForgotPasswordRepository
    {
        public ForgotPasswordViewModel InsertResetLink(ForgotPasswordViewModel _model);
        public bool ConfirmResetLink(string email, string token);
        public bool ResetPasswordLinkIsValid(string email, string token);
    }
}
