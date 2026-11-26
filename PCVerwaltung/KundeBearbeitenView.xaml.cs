using PCVerwaltung.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für KundeBearbeitenView.xaml
    /// </summary>
    public partial class KundeBearbeitenView : UserControl
    {
        // Event, damit der Host (MainView) die Meldung anzeigen kann
        public event Action<Kunde>? Saved;

        public KundeBearbeitenView()
        {
            InitializeComponent();

            cmbRanking.ItemsSource = Enum.GetValues(typeof(Ranking)).Cast<Ranking>();
            cmbZahlungsart.ItemsSource = Enum.GetValues(typeof(Zahlungsart)).Cast<Zahlungsart>();
            cmbKunde.ItemsSource = App.Kunden;

            if (App.Kunden.Count > 0)
                cmbKunde.SelectedIndex = 0;
        }

        private void OnKundeChanged(object sender, SelectionChangedEventArgs e) => FelderFuellen();

        private void OnAuswahlChanged(object sender, SelectionChangedEventArgs e) => AktualisiereHinweis();

        private void FelderFuellen()
        {
            ClearAllErrors();

            if (cmbKunde.SelectedItem is not Kunde kunde)
                return;

            txtKundenname.Text = kunde.Kundenname;
            txtStrasse.Text = kunde.Adresse.Strasse;
            txtPlz.Text = kunde.Adresse.Plz;
            txtOrt.Text = kunde.Adresse.Ort;
            txtEMail.Text = kunde.EMail;
            txtTelefon.Text = kunde.Telefon;
            cmbRanking.SelectedItem = kunde.Ranking;
            cmbZahlungsart.SelectedItem = kunde.Zahlungsart;

            AktualisiereHinweis();
        }

        private void AktualisiereHinweis()
        {
            // beim Aufbau ist noch nichts gewählt
            if (cmbRanking?.SelectedItem is not Ranking ranking ||
                cmbZahlungsart?.SelectedItem is not Zahlungsart zahlungsart ||
                txtHinweis is null)
            {
                return;
            }

            txtHinweis.Visibility = Visibility.Visible;

            if (!Kunde.IstErlaubt(ranking, zahlungsart))
            {
                txtHinweis.Foreground = ErrorBrush;
                txtHinweis.Text = Kunde.Begruendung(ranking, zahlungsart);
                return;
            }

            var erlaubt = string.Join(", ", Kunde.ErlaubteZahlungsarten(ranking));
            txtHinweis.Foreground = SystemColors.GrayTextBrush;
            txtHinweis.Text = $"Zulässig für Ranking {ranking}: {erlaubt}.";
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (cmbKunde.SelectedItem is not Kunde kunde)
            {
                MessageBox.Show("Bitte zuerst einen Kunden auswählen.",
                                "Keine Auswahl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ClearAllErrors();

            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtKundenname.Text))
            {
                SetError(txtKundenname, "Bitte Kundennamen angeben.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtStrasse.Text))
            {
                SetError(txtStrasse, "Bitte Straße angeben.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtPlz.Text))
            {
                SetError(txtPlz, "Bitte PLZ angeben.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtOrt.Text))
            {
                SetError(txtOrt, "Bitte Ort angeben.");
                ok = false;
            }

            if (!Kunde.IstGueltigeEMail(txtEMail.Text))
            {
                SetError(txtEMail, "Bitte eine gültige E-Mail-Adresse angeben.");
                ok = false;
            }

            if (cmbRanking.SelectedItem is not Ranking ranking)
            {
                SetError(cmbRanking, "Bitte Ranking auswählen.");
                ok = false;
                ranking = kunde.Ranking;
            }

            if (cmbZahlungsart.SelectedItem is not Zahlungsart zahlungsart)
            {
                SetError(cmbZahlungsart, "Bitte Zahlungsart auswählen.");
                ok = false;
                zahlungsart = kunde.Zahlungsart;
            }

            if (!ok) return;

            try
            {
                kunde.Adresse = new Adresse(txtStrasse.Text, txtPlz.Text, txtOrt.Text);
                kunde.EMail = txtEMail.Text;
                kunde.SetzeKonditionen(zahlungsart, ranking);
                kunde.Kundenname = txtKundenname.Text.Trim();
                kunde.Telefon = txtTelefon.Text.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Änderung nicht zulässig",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                AktualisiereHinweis();
                return;
            }

            Saved?.Invoke(kunde);

            // damit die Liste den neuen Namen zeigt
            cmbKunde.Items.Refresh();
            AktualisiereHinweis();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => FelderFuellen();

        #region Simple-Validation-Helpers
        private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60)); // Crimson

        private void SetError(Control c, string msg)
        {
            c.BorderBrush = ErrorBrush;
            c.BorderThickness = new Thickness(1.5);
            c.ToolTip = msg;
        }

        private void ClearError(Control c)
        {
            c.ClearValue(Border.BorderBrushProperty);
            c.ClearValue(Border.BorderThicknessProperty);
            c.ClearValue(ToolTipProperty);
        }

        private void ClearAllErrors()
        {
            ClearError(txtKundenname);
            ClearError(txtStrasse);
            ClearError(txtPlz);
            ClearError(txtOrt);
            ClearError(txtEMail);
            ClearError(cmbRanking);
            ClearError(cmbZahlungsart);
        }
        #endregion
    }
}
