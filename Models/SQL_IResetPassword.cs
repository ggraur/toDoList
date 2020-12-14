using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Models
{
    public class SQL_IForgotPassword : IForgotPassword
    {
        private readonly AppDbContext context;

        public SQL_IForgotPassword(AppDbContext context)
        {
            this.context = context;
        }

        public bool ConfirmResetLink(string email, string token)
        {
            bool bResult = false;
            List<ForgotPasswordViewModel> _model = context.ForgotPasswordViewModel.Where(c => c.Email == email && c.Token == token).ToList();
            if (_model.Count() > 0)
            {
                bResult = true;
            }
            return bResult;
        }

        public ForgotPasswordViewModel InsertResetLink(ForgotPasswordViewModel _model)
        {
            var allItems = context.ForgotPasswordViewModel.Where(c => c.Email == _model.Email && c.ResetLinkConfirmationDate == _model.ResetLinkConfirmationDate).AsQueryable();
            context.ForgotPasswordViewModel.RemoveRange(allItems);
            context.SaveChanges();

            context.ForgotPasswordViewModel.Add(_model);
            context.SaveChanges();
            return _model;
        }

        public bool ResetPasswordLinkIsValid(string email, string token)
        {
            bool bResult = false;
            //token = "CfDJ8AN7DtCXzUpKnfnw8kxZyiylcvgwrU0Wjy5bH5kP6RRIORcJHwZbjgYPdwI1fABDzF7jiq3ITEheZGT/iPuBpLRRqKsljtwRmUQb+u6RwfLxzUQHo8haCpbEz9eFPSr1YS417Y09Ligvs2bEZAwSizKBgxhlezTOpB/Czn+JqjT0EI86+pFQTJgV8NwKdOxlXYXZUDOC8EV/DzMhOOC7wOZOy0/Lp41Rkkdj8kubrNas";
            List<ForgotPasswordViewModel> _model = context.ForgotPasswordViewModel.Where(c => c.Email == email && c.Token == token).ToList();
            if (_model.Count() == 1) 
            {
                _model[0].ResetLinkConfirmationDate = DateTime.Now;

                var _task = context.ForgotPasswordViewModel.Attach(_model[0]);
                _task.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
  
                bResult = true;
            }
            return bResult;
        }
    }
}
