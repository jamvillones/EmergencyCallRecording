using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POSWPF.View.Stores;
using POSWPF.View.ViewModels;
using POSWPF.View.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWPF.View.Utilities {
    static class ServicesConfigurationManager {

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<RecordsStore>();

            services.AddTransient<RecordTabs>();
        }
    }
}
