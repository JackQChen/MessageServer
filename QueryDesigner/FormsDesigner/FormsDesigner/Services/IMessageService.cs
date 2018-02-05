namespace FormsDesigner.Services
{
    using System;

    public interface IMessageService
    {
        bool AskQuestion(string question);
        bool AskQuestion(string question, string caption);
        bool AskQuestionFormatted(string formatstring, params string[] formatitems);
        bool AskQuestionFormatted(string caption, string formatstring, params string[] formatitems);
        int ShowCustomDialog(string caption, string dialogText, params string[] buttontexts);
        void ShowError(Exception ex);
        void ShowError(string message);
        void ShowError(Exception ex, string message);
        void ShowErrorFormatted(string formatstring, params string[] formatitems);
        void ShowMessage(string message);
        void ShowMessage(string message, string caption);
        void ShowMessageFormatted(string formatstring, params string[] formatitems);
        void ShowMessageFormatted(string caption, string formatstring, params string[] formatitems);
        void ShowWarning(string message);
        void ShowWarningFormatted(string formatstring, params string[] formatitems);
    }
}

