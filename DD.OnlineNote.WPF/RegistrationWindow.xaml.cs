using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace DD.OnlineNote.WPF
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window 
    {
        private ServiceProvider provider;
        public RegistrationWindow()
        {
            InitializeComponent();
            provider = ServiceProvider.GetProvider();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                EmptyLogin.IsOpen = true;
                return;
            }
            else if(string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(PasswordRepeatBox.Password))
            {
                empryPassword.IsOpen = true;
                return;
            }
            else if (PasswordBox.Password != PasswordRepeatBox.Password)
            {
                passwordDontmach.IsOpen = true;
                return;
            }
            else if(await provider.CheckLogin(LoginBox.Text))
            {
                UserAlredyExist.IsOpen = true;
                return;
            }
            User usr = new User() { Name = LoginBox.Text , Password = PasswordBox.Password };
            usr = await provider.CreateUser(usr);

            if (usr == null || usr.Id == default(Guid))
            {
                CreateUserError.IsOpen = true;
                return;
            }
            var own = Owner as LoginWindow;
            if(own != null)
                own.loginBox.Text = usr.Name;
            Close();

        }

        private async void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
                return;

            if (await provider.CheckLogin(LoginBox.Text))
            {
                LoginBox.Background = Brushes.LightCoral;
                UserAlredyExist.IsOpen = true;
                return;
            }
        }
    }
    class UserToCreate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string login;
        private string password;
        private string passwordRepeat;

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged("LoginBox");
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("PasswordBox");
            }
        }
        public string PasswordRepeat
        {
            get
            {
                return passwordRepeat;
            }
            set
            {
                passwordRepeat = value;
                OnPropertyChanged("PasswordRepeatBox");
            }
        }
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        

    }
}
