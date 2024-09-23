using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWPF.View.Utilities {
    interface IViewModelFactory {
        T Get<T>() where T : ObservableObject;
    }
    sealed class ViewModelFactory : IViewModelFactory {
        public T Get<T>() where T : ObservableObject =>
            App.Host.Services.GetRequiredService<T>();

    }
}
