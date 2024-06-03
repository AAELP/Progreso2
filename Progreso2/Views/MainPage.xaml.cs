using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Progreso2.Models;

namespace Progreso2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnRecargaChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var radioButton = sender as RadioButton;
                DisplayAlert("Monto de Recarga", $"Has seleccionado {radioButton.Content}", "OK");
            }
        }

        private async void OnRecargarClicked(object sender, EventArgs e)
        {
            var recarga = new Recargas
            {
                NumeroTelefonico = ABTelefonoEntry.Text,
                Operador = ABOperadorPicker.SelectedItem?.ToString(),
                Monto = GetSelectedRecarga(),
                Fecha = DateTime.Now
            };

            if (string.IsNullOrEmpty(recarga.NumeroTelefonico) || string.IsNullOrEmpty(recarga.Operador) || string.IsNullOrEmpty(recarga.Monto))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            bool confirmacion = await DisplayAlert("Confirmar Recarga", $"¿Desea recargar {recarga.Monto} dólares al número {recarga.NumeroTelefonico} con el operador {recarga.Operador}?", "Sí", "No");
            if (confirmacion)
            {
                await RealizarRecargaAsync(recarga);
            }
        }

        private string GetSelectedRecarga()
        {
            if (ABRecarga3.IsChecked)
                return "3";
            if (ABRecarga5.IsChecked)
                return "5";
            if (ABRecarga10.IsChecked)
                return "10";
            return null;
        }

        private async Task RealizarRecargaAsync(Recargas recarga)
        {
            string contenido = $"Se hizo una recarga de {recarga.Monto} dólares en la siguiente fecha; {recarga.Fecha:dd/MM/yyyy}";

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{recarga.NumeroTelefonico}.txt");

            File.WriteAllText(path, contenido);

            await DisplayAlert("Recarga Exitosa", "Su recarga se ha realizado correctamente.", "OK");
        }
    }
}
