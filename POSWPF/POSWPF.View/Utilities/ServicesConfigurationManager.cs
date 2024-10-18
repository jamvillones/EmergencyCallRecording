using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECR.View.Stores;
using ECR.View.ViewModels;
using ECR.View.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECR.WPF.ViewModels;
using ECR.Domain.Data;
using ECR.View.ViewModels.Contents;

namespace ECR.View.Utilities {
    static class ServicesConfigurationManager {

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<NotificationHandler>();
            services.AddSingleton<ModalContentManager>();
            services.AddSingleton<RecordsStore>();

            services.AddTransient<IDBContextFactory, DbContextFactory>();
            services.AddTransient<MainContentViewModel>();
            services.AddTransient<RecordTabs>();

            services.AddTransient<Records_CallsSection>();
            services.AddTransient<Records_AgenciesSection>();

            services.AddTransient<Form_Add_Record_ViewModel>();
            services.AddTransient<Form_Edit_Record_ViewModel>();
            services.AddTransient<Form_Add_Agency_ViewModel>();
            services.AddTransient<Form_Edit_Agency_ViewModel>();

            services.AddTransient<Agency_Item_ViewModel>();
            services.AddTransient<Record_Item_ViewModel>();
        }
    }
}
