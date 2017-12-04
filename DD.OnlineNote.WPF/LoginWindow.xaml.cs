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
using System.Windows.Media.Effects;

namespace DD.OnlineNote.WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private ServiceProvider provider;
        Brush currentBrush;
        public LoginWindow()
        {
            InitializeComponent();
            currentBrush = this.Background;
            provider = ServiceProvider.GetProvider();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginBox.Text))
            {
                popupNotFillLogin.IsOpen = true;
            }
            else if(string.IsNullOrEmpty(passwordBox.Password))
            {
                popupNotFillPassword.IsOpen = true;
            }

            User usr = await provider.Login(new User { Name = loginBox.Text, Password = passwordBox.Password });
            if (usr != null)
            {
                Main main = new Main(usr.Id);
                main.Title = $"OnlineNote, user: {loginBox.Text}";
                main.Show();
                //popupWelcom.IsOpen = true;
                this.Close();
            }
            else
            {
                popupIncorretUserOrPswd.IsOpen = true;
            }

        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            BlurEffect blur = new BlurEffect();
            blur.Radius = 5;
            //this.Background = new SolidColorBrush(Colors.DarkGray);
            this.Effect = blur;
            RegistrationWindow RegWin = new RegistrationWindow();
            RegWin.Owner = this;
            RegWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            RegWin.ShowDialog();
            this.Effect = null;
            this.Background = currentBrush;

           
                        
        }
    }
}
