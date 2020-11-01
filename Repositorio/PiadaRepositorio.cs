using DanielDevBot.Model;
using DanielDevBot.Utils;
using Dapper;
using System.Data.SQLite;

namespace DanielDevBot.Repositorio
{
    public class PiadaRepositorio
    {
        SQLiteConnection con;

        public PiadaRepositorio()
        {
            con = new SQLiteConnection($@"URI={BotAppSettings.DBPath}piadas.sqlite");
        }

        public PiadaModel ObterPiadaAleatoria()
        {
            con.Open();
            var piada = con.QueryFirst<PiadaModel>("SELECT Pergunta, Resposta FROM `charadas` ORDER BY RANDOM() LIMIT 1;");
            con.Close();
            return piada;
        }
    }
}
