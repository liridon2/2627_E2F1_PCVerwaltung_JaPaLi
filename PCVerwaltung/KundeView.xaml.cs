using PCVerwaltung.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für KundeView.xaml
    /// </summary>
    public partial class KundeView : UserControl
    {
        // Event, damit der Host (MainView) das Ergebnis bekommt
        public event Action<Kunde>? Saved;
        public event Action? Cancelled;

        public KundeView()
        {
            InitializeComponent();

            cmbRanking.ItemsSource = Enum.GetValues(typeof(Ranking)).Cast<Ranking>();
            cmbZahlungsart.ItemsSource = Enum.GetValues(typeof(Zahlungsart)).Cast<Zahlungsart>();

            ResetFields();
        }

        private void OnAuswahlChanged(object sender, SelectionChangedEventArgs e) => AktualisiereHinweis();

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
                ranking = Ranking.STANDARD;
            }

            if (cmbZahlungsart.SelectedItem is not Zahlungsart zahlungsart)
            {
                SetError(cmbZahlungsart, "Bitte Zahlungsart auswählen.");
                ok = false;
                zahlungsart = Zahlungsart.Bar;
            }

            if (!ok) return;

            Kunde data;
            try
            {
                var adresse = new Adresse(txtStrasse.Text, txtPlz.Text, txtOrt.Text);
                data = new Kunde(txtKundenname.Text, adresse, txtEMail.Text, txtTelefon.Text,
                                 zahlungsart, ranking);
            }
            catch (Exception ex)
            {
                SetError(cmbZahlungsart, ex.Message);
                MessageBox.Show(ex.Message, "Eingabe nicht zulässig",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                AktualisiereHinweis();
                return;
            }

            // An Host signalisieren (zeigt u.a. die Erfolgsmeldung an)
            Saved?.Invoke(data);

            // Felder zurücksetzen (optional)
            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            ResetFields();
            Cancelled?.Invoke();
        }

        private void ResetFields()
        {
            txtKundenname.Text = "";
            txtStrasse.Text = "";
            txtPlz.Text = "";
            txtOrt.Text = "";
            txtEMail.Text = "";
            txtTelefon.Text = "";
            cmbRanking.SelectedItem = Ranking.STANDARD;
            cmbZahlungsart.SelectedItem = Zahlungsart.Bar;
            ClearAllErrors();
            AktualisiereHinweis();
        }

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
