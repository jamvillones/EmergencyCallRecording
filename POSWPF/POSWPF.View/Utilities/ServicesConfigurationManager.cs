using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECR.View.Stores;
using ECR.View.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR.WPF.ViewModels;
using ECR.Domain.Data;
using ECR.View.ViewModels.Contents;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels.Tabs;
using ECR.WPF.Views;

namespace ECR.View.Utilities {
    static class ServicesConfigurationManager {

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<INotificationHandler, NotificationHandler>();
            services.AddTransient<IDBContextFactory, DbContextFactory>();
            services.AddSingleton<ILoginHandler, LoginHandler>();
            services.AddSingleton<IModalViewer, ModalViewer>();


            services.AddSingleton<MainViewModel>();
            services.AddSingleton<RecordsStore>();

            services.AddTransient<MainContentViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RecordTabs>();
            services.AddTransient<Login_Tab>();

            services.AddTransient<Records_CallsSection>();
            services.AddTransient<Records_AgenciesSection>();

            services.AddTransient<Form_Add_Record_ViewModel>();
            services.AddTransient<Form_Edit_Record_ViewModel>();
            services.AddTransient<Form_Add_Agency_ViewModel>();
            services.AddTransient<Form_Edit_Agency_ViewModel>();
            services.AddTransient<AuthenticationForm_ViewModel>();

            services.AddTransient<Agency_Item_ViewModel>();
            services.AddTransient<Record_Item_ViewModel>();
            services.AddTransient<Contact_Item_ViewModel>();

            services.AddTransient<SignUp_Form_ViewModel>();
            services.AddTransient<EditLogin_Form_ViewModel>();
            services.AddTransient<IPaginator, Paginator>();

            services.AddKeyedTransient<IPasswordHandler, Register_Login_PasswordHandler>(FormSaveType.Register);
            services.AddKeyedTransient<IPasswordHandler, Edit_Login_PasswordHandler>(FormSaveType.Edit);
        }
    }
}
