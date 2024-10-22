﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace ECR.WPF.ViewModels.Tabs {
    sealed partial class Login_Tab : ObservableObject {
        public Login_Tab() {

        }

        public Login_Tab(IDBContextFactory dBContextFactory, IViewModelFactory viewModelFactory, IModalViewer modalViewer, ILoginHandler loginHandler) {
            DBContextFactory = dBContextFactory;
            ViewModelFactory = viewModelFactory;
            ModalViewer = modalViewer;
            LoginHandler = loginHandler;

            _ = LoadDataAsync();
        }

        private string? _keyword = null;
        public ObservableCollection<Login> Logins { get; } = [];
        public IDBContextFactory DBContextFactory { get; }
        public IViewModelFactory ViewModelFactory { get; }
        public IModalViewer ModalViewer { get; }
        public ILoginHandler LoginHandler { get; }

        async Task LoadDataAsync(string? keyword = null) {
            try {
                using var context = DBContextFactory.CreateDbContext();
                var logins = await context.Logins.AsNoTracking().AsQueryable()
                    .Where(l => l.Username != "admin")
                    .FilterLogin(keyword)
                    .OrderBy(l => l.Name.First)
                        .ThenBy(l => l.Name.Last)
                    .ToListAsync();

                if (logins.Count != 0) Logins.Clear();
                foreach (var login in logins)
                    Logins.Add(login);

            }
            catch (Exception) {

                throw;
            }
        }

        [RelayCommand]
        async Task Search(string text) {

            _keyword = text.Trim();
            await LoadDataAsync(_keyword);
        }

        [RelayCommand]
        async Task Reset() {
            _keyword = "";
            await LoadDataAsync(_keyword);
        }

        [RelayCommand]
        void OpenRegistrationForm() {
            var registration = ViewModelFactory.Get<SignUp_Form_ViewModel>();
            registration.OnSave += (a, b) => Logins.Add(b);
            ModalViewer.SetModal(registration);
        }

        [RelayCommand]
        async Task RemoveLogin(Login login) {
            if (!LoginHandler.IsAdmin) {
                MessageBox.Show("You do not have permission to do this action!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("This action cannot be undone!", "Delete Login Credentials?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;

            try {
                using var context = DBContextFactory.CreateDbContext();
                context.Entry(login).State = EntityState.Deleted;

                await context.SaveChangesAsync();
                Logins.Remove(login);
            }
            catch (Exception) {


            }

        }
    }
}
