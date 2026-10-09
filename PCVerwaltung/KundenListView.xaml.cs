using PCVerwaltung.Classes;
using System.Windows;
using System.Windows.Controls;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für KundenListView.xaml
    /// </summary>
    public partial class KundenListView : UserControl
    {
        public KundenListView()
        {
            InitializeComponent();
        }

        private void OnKundeLoeschen(object sender, RoutedEventArgs e)
        {
            if (dgKunden.SelectedItem is not Kunde kunde)
            {
                MessageBox.Show("Bitte zuerst einen Kunden auswählen.");
                return;
            }

            // ObservableCollection aktualisiert das DataGrid automatisch
            App.Kunden.Remove(kunde);
        }
    }
}
