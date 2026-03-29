using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Aplikacja_1_Wypozyczalnia_Sklep_Gier
{
    /// <summary>
    /// Logika interakcji dla klasy LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string connString = @"Data Source=DESKTOP-E2DP3VT\SQLEXPRESS; Initial Catalog=SystemSklep; Integrated Security=True; TrustServerCertificate=True;";

            using (SqlConnection sqlCon = new SqlConnection(connString))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                        sqlCon.Open();

                    string query = "SELECT Rola FROM Uzytkownicy WHERE Login=@Login AND Haslo=@Haslo";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.CommandType = CommandType.Text;

                    sqlCmd.Parameters.AddWithValue("@Login", txtUsername.Text);
                    sqlCmd.Parameters.AddWithValue("@Haslo", txtPassword.Password);

                    object result = sqlCmd.ExecuteScalar();

                    if (result != null)
                    {
                        string rolaUzytkownika = result.ToString();


                        if (rolaUzytkownika == "Admin")
                        {
                            MessageBox.Show("Logowanie zakończone sukcesem: Panel Administratora");


                            AdminWindow aw = new AdminWindow();
                            aw.Show();
                        }
                        else if (rolaUzytkownika == "Gracz" || rolaUzytkownika == "Nowy_Gracz")
                        {
                            MessageBox.Show($"Logowanie zakończone sukcesem: Panel Użytkownika ({rolaUzytkownika})");


                            UserWindow uw = new UserWindow();
                            uw.Show();
                        }
                        else
                        {

                            MessageBox.Show("Zalogowano, ale nie przypisano widoku dla tej roli.");
                            MainWindow mw = new MainWindow();
                            mw.Show();
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Błędny login lub hasło!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia: " + ex.Message);
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Show();
                }
            }
        }
    }
}
