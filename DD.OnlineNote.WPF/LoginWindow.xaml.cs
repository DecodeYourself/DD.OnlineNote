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
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                MessageBox.Show("Заполните имя пользователя");
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
