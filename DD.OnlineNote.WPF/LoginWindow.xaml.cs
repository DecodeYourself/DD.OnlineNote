using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DD.OnlineNote.Model;
using System.Windows.Controls.Primitives;

namespace DD.OnlineNote.WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ServiceProvider provider;

        public LoginWindow()
        {
            InitializeComponent();
#if DEBUG
            provider = ServiceProvider.GetProvider("http://localhost:62140/");
#else
            provider = ServiceProvider.GetProvider("http://onlinenote.azurewebsites.net/");
#endif
        }

        private async  void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                popupFillLogin.IsOpen = true;
                return;
            }
            if( await provider.CheckLogin(txtLogin.Text))
            {
                popupWelcom.IsOpen = true;
            }
            else
            {
                popupNotFoundUser.IsOpen = true;
                return;
            }

        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Заполните имя пользователя");
                return;
            }
        }
    }
}
