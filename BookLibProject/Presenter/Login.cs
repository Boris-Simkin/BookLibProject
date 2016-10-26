using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibProject.Presenter
{
    public class Login
    {
        private readonly ILoginView _view;
        private readonly IUsers _model;

        public Login(ILoginView mainWindowVeiw, IUsers users)
        {
            _view = mainWindowVeiw;
            _model = users;
            _view.Submit += _view_Submit;
        }

        private async void _view_Submit(object sender, SubmitEventArgs e)
        {
           Users.AuthenticationResult authentication = await _model.Authentication(e.Username, e.Password);

            if (authentication == Users.AuthenticationResult.IsUser)
            {
                _view.SetUserPage();
            }

            if (authentication == Users.AuthenticationResult.IsAdmin)
            {
                _view.SetAdminPage();
            }
        }
    }
}
