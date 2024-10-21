using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.ViewModels.Contents;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.Utilities {
    internal class LoginHandler : ILoginHandler {
        public LoginHandler(IDBContextFactory dBContextFactory) {
            DBContextFactory = dBContextFactory;
        }

        public LoginStatusType LoginStatus { get; private set; } = LoginStatusType.Pending;
        public Login? Login { get; private set; } = null!;

        public IDBContextFactory DBContextFactory { get; }

        public async Task<bool> TryLoginAsync(string username, string password) {
            LoginStatus = LoginStatusType.Pending;

            try {
                using var context = DBContextFactory.CreateDbContext();
                Login = await context.Logins.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
                await Task.Delay(1000);
                LoginStatus = Login is null ? LoginStatusType.Failed : LoginStatusType.Okay;

            }
            catch {
                LoginStatus = LoginStatusType.Disconnected;
                return false;
            }

            return LoginStatus != LoginStatusType.Failed;
        }


    }
}
