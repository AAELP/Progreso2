using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Storage;

namespace Progreso2.Models
{
    internal class AllRecargas
    {
        public ObservableCollection<Recargas> Recargas { get; set; } = new ObservableCollection<Recargas>();
        private readonly string filePath;

        public AllRecargas()
        {
            // Define el nombre del archivo donde se guardarán las recargas
            filePath = Path.Combine(FileSystem.AppDataDirectory, "RegistroRecarga.txt");
            LoadRecargas();
        }

        public void LoadRecargas()
        {
            Recargas.Clear();

            // Verifica si el archivo existe antes de intentar leerlo
            if (File.Exists(filePath))
            {
                // Lee todas las líneas del archivo y convierte cada línea en una instancia de Recarga
                IEnumerable<Recargas> recargas = File.ReadAllLines(filePath)
                                                     .Select(line => ParseRecarga(line))
                                                     .Where(recarga => recarga != null)
                                                     .OrderBy(recarga => recarga.Fecha);

                // Agrega cada recarga a la colección observable
                foreach (Recargas recarga in recargas)
                    Recargas.Add(recarga);
            }
        }

        public void AddRecarga(Recargas recarga)
        {
            // Agrega la nueva recarga a la colección
            Recargas.Add(recarga);

            // Guarda la nueva recarga en el archivo
            string registro = $"{recarga.NumeroTelefonico}|{recarga.Operador}|{recarga.Monto}|{recarga.Fecha:dd/MM/yyyy}";
            File.AppendAllText(filePath, registro + Environment.NewLine);
        }

        private Recargas ParseRecarga(string line)
        {
            // Divide la línea en partes usando el separador '|'
            string[] parts = line.Split('|');
            if (parts.Length == 4)
            {
                return new Recargas
                {
                    NumeroTelefonico = parts[0],
                    Operador = parts[1],
                    Monto = parts[2],
                    Fecha = DateTime.Parse(parts[3])
                };
            }
            return null;
        }
    }
}
