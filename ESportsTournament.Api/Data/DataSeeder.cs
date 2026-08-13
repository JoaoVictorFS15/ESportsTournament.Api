using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Data
{
    public class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // 1. Limpa as tabelas na ordem correta (filhos primeiro)
            context.Equipes.RemoveRange(context.Equipes);
            context.Torneios.RemoveRange(context.Torneios);
            context.Usuarios.RemoveRange(context.Usuarios);

            // Força o salvamento da exclusão
            context.SaveChanges();
            // 2. Cria Usuários (Organizadores e Capitães fictícios)
            // A senha para todos será "123"
            var org = new Usuario { Nome = "Admin Organizador", Nick = "Admin", Email = "admin@org.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Organizador" };
            var capitao1 = new Usuario { Nome = "Gabriel Toledo", Nick = "Fallen", Email = "fallen@cs.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Capitao" };
            var capitao2 = new Usuario { Nome = "Lee Sang-hyeok", Nick = "Faker", Email = "faker@lol.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Capitao" };
            var jogadorComum = new Usuario { Nome = "Jogador Teste", Nick = "NoobMaster", Email = "teste@teste.com", SenhaHash = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Jogador" };

            context.Usuarios.AddRange(org, capitao1, capitao2, jogadorComum);
            context.SaveChanges(); // Salva no banco para gerar os Ids!
            // 3. Cria Torneios Falsos
            var torneioCs = new Torneio { Nome = "IEM Rio Major", Jogo = "CS2", DataInicio = DateTime.Now.AddDays(10), DataFim = DateTime.Now.AddDays(20), Premiacao = 1000000, Status = "Aberto" };
            var torneioLol = new Torneio { Nome = "CBLOL", Jogo = "League of Legends", DataInicio = DateTime.Now.AddDays(5), DataFim = DateTime.Now.AddDays(30), Premiacao = 500000, Status = "Aberto" };
            context.Torneios.AddRange(torneioCs, torneioLol);
            context.SaveChanges();
            // 4. Cria Equipes Falsas já linkadas aos Capitães e Torneios
            var equipe1 = new Equipe { Nome = "Imperial Esports", Abreviacao = "IMP", TorneioId = torneioCs.Id, CapitaoId = capitao1.Id };
            var equipe2 = new Equipe { Nome = "T1", Abreviacao = "T1", TorneioId = torneioLol.Id, CapitaoId = capitao2.Id };
            var equipeSemTorneio = new Equipe { Nome = "Equipe Sem Rumo", Abreviacao = "ESR", TorneioId = null, CapitaoId = jogadorComum.Id };
            context.Equipes.AddRange(equipe1, equipe2, equipeSemTorneio);
            context.SaveChanges();
        }
    }
}
