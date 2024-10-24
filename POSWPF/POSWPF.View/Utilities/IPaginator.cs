namespace ECR.WPF.Utilities {
    public interface IPaginator {
        bool CanNext { get; }
        bool CanPrevious { get; }
        int CurrentPage { get; set; }
        int MaxPages { get; }
        int StartIndex { get; }
        int PageSize { get; }

        void CalculateMaxPages(int totalCount);
        void First();
        void Last();
        void Next();
        void Previous();

        event EventHandler<int>? OnPageChanged;
        event EventHandler? PageSizeChanged;
    }
}