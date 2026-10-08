using PCVerwaltung.Classes;
using System;
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
            cmbZahlungsart.ItemsSource = Enum.GetValues(typeof(Finanzierung)).Cast<Finanzierung>();
            cmbZahlungsart.SelectedItem = Finanzierung.Rechnung;
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

            if (string.IsNullOrWhiteSpace(txtRanking.Text))
            {
                SetError(txtRanking, "Bitte Ranking angeben.");
                ok = false;
            }

            if (!ok) return;

            var data = new Kunde(txtKundenname.Text.Trim(), (Finanzierung)cmbZahlungsart.SelectedItem, txtRanking.Text.Trim());

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
            txtRanking.Text = "";
            cmbZahlungsart.SelectedItem = Finanzierung.Rechnung;
            ClearAllErrors();
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
            ClearError(txtRanking);
        }
        #endregion
    }
}
