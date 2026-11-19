using PCVerwaltung.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für ZahlungsartView.xaml
    /// </summary>
    public partial class ZahlungsartView : UserControl
    {
        // Event, damit der Host (MainView) die Meldung anzeigen kann
        public event Action<Kunde>? Saved;

        public ZahlungsartView()
        {
            InitializeComponent();

            cmbZahlungsart.ItemsSource = Enum.GetValues(typeof(Zahlungsart)).Cast<Zahlungsart>();
            cmbKunde.ItemsSource = App.Kunden;

            if (App.Kunden.Count > 0)
                cmbKunde.SelectedIndex = 0;
        }

        private Kunde? AktuellerKunde => cmbKunde.SelectedItem as Kunde;

        private void OnKundeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AktuellerKunde is not Kunde kunde)
            {
                txtRanking.Text = "";
                txtHinweis.Visibility = Visibility.Collapsed;
                return;
            }

            txtRanking.Text = kunde.Ranking.ToString();
            cmbZahlungsart.SelectedItem = kunde.Zahlungsart;
            AktualisiereHinweis();
        }

        private void OnZahlungsartChanged(object sender, SelectionChangedEventArgs e) => AktualisiereHinweis();

        private void AktualisiereHinweis()
        {
            // beim Aufbau ist noch nichts gewählt
            if (AktuellerKunde is not Kunde kunde ||
                cmbZahlungsart?.SelectedItem is not Zahlungsart gewaehlt ||
                txtHinweis is null)
            {
                return;
            }

            txtHinweis.Visibility = Visibility.Visible;

            if (!Kunde.IstErlaubt(kunde.Ranking, gewaehlt))
            {
                txtHinweis.Foreground = ErrorBrush;
                txtHinweis.Text = Kunde.Begruendung(kunde.Ranking, gewaehlt);
                return;
            }

            var erlaubt = string.Join(", ", Kunde.ErlaubteZahlungsarten(kunde.Ranking));
            txtHinweis.Foreground = SystemColors.GrayTextBrush;
            txtHinweis.Text = $"Zulässig für Ranking {kunde.Ranking}: {erlaubt}.";
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (AktuellerKunde is not Kunde kunde)
            {
                MessageBox.Show("Bitte zuerst einen Kunden auswählen.",
                                "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbZahlungsart.SelectedItem is not Zahlungsart gewaehlt)
            {
                MessageBox.Show("Bitte eine Zahlungsart auswählen.",
                                "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                kunde.Zahlungsart = gewaehlt;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Zahlungsart nicht zulässig",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                AktualisiereHinweis();
                return;
            }

            Saved?.Invoke(kunde);
            AktualisiereHinweis();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // zurück auf den gespeicherten Stand
            if (AktuellerKunde is Kunde kunde)
                cmbZahlungsart.SelectedItem = kunde.Zahlungsart;

            AktualisiereHinweis();
        }

        private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60)); // Crimson
    }
}
