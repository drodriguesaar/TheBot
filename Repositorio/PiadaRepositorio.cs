using DanielDevBot.Model;
using Dapper;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace DanielDevBot.Repositorio
{
    public class PiadaRepositorio
    {
        SQLiteConnection con;

        public PiadaRepositorio()
        {
            con = new SQLiteConnection(@"URI=file:H:\Labs\DanielDevBot\DB\piadas.sqlite");
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
