using ECR.Domain.Models;
using ECR.View.ViewModels.Contents;

namespace ECR.WPF.Utilities {
    public interface ILoginHandler {
        Login? Login { get; }

        bool IsAdmin { get; }
        LoginStatusType LoginStatus { get; }

        Task<bool> TryLoginAsync(string username, string password);
    }
}