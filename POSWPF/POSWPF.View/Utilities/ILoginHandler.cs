using ECR.Domain.Models;
using ECR.View.ViewModels.Contents;

namespace ECR.WPF.Utilities {
    internal interface ILoginHandler {
        Login? Login { get; }
        LoginStatusType LoginStatus { get; }

        Task<bool> TryLoginAsync(string username, string password);
    }
}