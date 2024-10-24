using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.Utilities {
    public sealed partial class Paginator : ObservableObject, IPaginator {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextCommand))]
        private int _maxPages = 1;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextCommand))]
        private int _currentPage = 1;

        [ObservableProperty]
        int _totalCount;

        public event EventHandler<int>? OnPageChanged;

        public void CalculateMaxPages(int totalCount) {
            MaxPages = (int)Math.Ceiling((double)totalCount / (double)PageSize);
            TotalCount = totalCount;
        }

        public bool CanNext => CurrentPage < MaxPages;
        public bool CanPrevious => CurrentPage > 1;

        [RelayCommand(CanExecute = nameof(CanPrevious))]
        public void First() {
            CurrentPage = 1;
            OnPageChanged?.Invoke(this, CurrentPage);
        }

        [RelayCommand(CanExecute = nameof(CanNext))]
        public void Last() {
            CurrentPage = MaxPages;
            OnPageChanged?.Invoke(this, CurrentPage);
        }

        [RelayCommand(CanExecute = nameof(CanNext))]
        public void Next() {
            CurrentPage++;
            OnPageChanged?.Invoke(this, CurrentPage);

        }

        [RelayCommand(CanExecute = nameof(CanPrevious))]
        public void Previous() {
            CurrentPage--;
            OnPageChanged?.Invoke(this, CurrentPage);
        }

        public int StartIndex => (CurrentPage - 1) * PageSize;

        [ObservableProperty]
        int _pageSize = 100;

        partial void OnPageSizeChanged(int value) {
            PageSizeChanged?.Invoke(this, EventArgs.Empty);
        }


        public event EventHandler? PageSizeChanged;
    }
}
