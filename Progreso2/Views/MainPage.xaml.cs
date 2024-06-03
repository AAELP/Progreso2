using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Progreso2.Models;

namespace Progreso2
{
    public partial class MainPage : ContentPage
    {
        private AllRecargas allRecargas = new AllRecargas();

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
            allRecargas.AddRecarga(recarga);
            await DisplayAlert("Recarga Exitosa", "Su recarga se ha realizado correctamente.", "OK");
        }

        private async void OnVerRegistroClicked(object sender, EventArgs e)
        {
            if (allRecargas.Recargas.Count > 0)
            {
                string contenido = string.Join(Environment.NewLine, allRecargas.Recargas
                    .Select(r => $"Número: {r.NumeroTelefonico}, Operador: {r.Operador}, Monto: {r.Monto}, Fecha: {r.Fecha:dd/MM/yyyy}"));
                await DisplayAlert("Registro de Recargas", contenido, "OK");
            }
            else
            {
                await DisplayAlert("Registro de Recargas", "No hay registros disponibles.", "OK");
            }
        }
    }
}
