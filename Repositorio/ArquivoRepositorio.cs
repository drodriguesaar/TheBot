using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielDevBot.Repositorio
{
    public class ArquivoRepositorio
    {
        readonly string _PathArquivo;
        public ArquivoRepositorio(string PathArquivo)
        {
            _PathArquivo = PathArquivo;
        }

        public bool InserirLinhas(IEnumerable<object> dados)
        {
            try
            {
                using (var stream = File.Open(_PathArquivo, FileMode.Append))
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = false
                }))
                {
                    csv.WriteRecords(dados);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
