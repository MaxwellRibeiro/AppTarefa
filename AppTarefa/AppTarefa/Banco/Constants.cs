using System;
using System.IO;

namespace AppTarefa.Banco
{
    public static class Constants
    {
        public const string NomeDoArquivo = "AppTarefa.db3";

        public static string CaminhoDoBanco
        {
            get
            {
                var caminhoBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(caminhoBase, NomeDoArquivo);
            }
        }
    }
}
