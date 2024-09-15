using System;
using System.Windows;
using SanctuarySSLib.MiscUtil;
using WMessageBox = System.Windows.MessageBox;

namespace SanctuarySSModManager.MiscUtil
{
    public class UserInteraction : IUserInteraction
    {

        public UserInteraction() 
        {
        } 
        public UserInteractionResult MessageBox(string caption, string message, UserInteractionButton buttons)
        {
            var result = WMessageBox.Show(caption, message, (MessageBoxButton)buttons);
            return (UserInteractionResult)result;
        }
    }
}
