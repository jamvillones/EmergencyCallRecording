using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.View.Utilities {
    public interface IViewModelFactory {
        T Get<T>() where T : ObservableObject;
    }
    public sealed class ViewModelFactory : IViewModelFactory {
        public T Get<T>() where T : ObservableObject =>
            App.Host.Services.GetRequiredService<T>();

    }
}
