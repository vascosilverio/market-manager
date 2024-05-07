using market_manager.Data;
using market_manager.Models;
using Microsoft.EntityFrameworkCore;
using static market_manager.Models.Bancas;
using static market_manager.Models.Notificacoes;
using static market_manager.Models.Reservas;

namespace market_manager.Models.Seed;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {

            // Verifica se há dados na base de dados
            if (context.Gestores.Any() || context.Vendedores.Any() || context.Reservas.Any() || context.Notificacoes.Any() || context.Bancas.Any())
            {
                return;
            }

            // Adiciona bancas
            context.Bancas.AddRange(
                new Bancas
                {
                    NomeIdentificadorBanca = "A01",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 3.5m,
                    Largura = 2.0m,
                    LocalizacaoX = 10,
                    LocalizacaoY = 20,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "B02",
                    CategoriaBanca = CategoriaProdutos.Refrigerados,
                    Comprimento = 4.0m,
                    Largura = 2.5m,
                    LocalizacaoX = 30,
                    LocalizacaoY = 15,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "C03",
                    CategoriaBanca = CategoriaProdutos.Peixe,
                    Comprimento = 3.0m,
                    Largura = 2.0m,
                    LocalizacaoX = 50,
                    LocalizacaoY = 25,
                    EstadoAtualBanca = EstadoBanca.Livre
                },
                new Bancas
                {
                    NomeIdentificadorBanca = "D04",
                    CategoriaBanca = CategoriaProdutos.Frescos,
                    Comprimento = 4.5m,
                    Largura = 3.0m,
                    LocalizacaoX = 20,
                    LocalizacaoY = 40,
                    EstadoAtualBanca = EstadoBanca.Ocupada
                }
            );


            // Adiciona gestores
            context.Gestores.AddRange(
                new Gestores
                {
                    PrimeiroNome = "Pedro",
                    UltimoNome = "Parreira",
                    CC = "123456789",
                    NIF = "123456789",
                    NumIdFuncionario = "001",
                    DataNascimento = new DateOnly(1980, 5, 15),
                    DataAdmissao = new DateOnly(2010, 1, 1),
                    Morada = "Rua das Flores, 123",
                    Localidade = "Lisboa",
                    CodigoPostal = "1234-567",
                    Telemovel = "912345678"
                },
                new Gestores
                {
                    PrimeiroNome = "Maria",
                    UltimoNome = "Rodrigues",
                    CC = "987654321",
                    NIF = "987654321",
                    NumIdFuncionario = "002",
                    DataNascimento = new DateOnly(1985, 10, 20),
                    DataAdmissao = new DateOnly(2015, 7, 1),
                    Morada = "Avenida da Liberdade, 456",
                    Localidade = "Porto",
                    CodigoPostal = "4567-890",
                    Telemovel = "987654321"
                }
            );

            // Adiciona vendedor
            var novoVendedor = context.Vendedores.Add(
                new Vendedores
                {
                    PrimeiroNome = "Pedro",
                    UltimoNome = "Ferreira",
                    CC = "456789012",
                    NIF = "456789012",
                    NISS = "12345678901",
                    DocumentoCartaoComerciante = "documento_cartao.pdf",
                    DataNascimento = new DateOnly(1990, 3, 10),
                    Morada = "Rua do Comércio, 789",
                    Localidade = "Braga",
                    CodigoPostal = "4567-890",
                    Telemovel = "965432109",
                    EstadoActualRegisto = Vendedores.EstadoRegisto.Aprovado
                }
            ).Entity;

            // Adiciona reservas
            context.Reservas.AddRange(
                new Reservas
                {
                    DataInicio = new DateOnly(2023, 6, 1),
                    DataFim = new DateOnly(2023, 6, 15),
                    DataCriacao = DateTime.Now,
                    EstadoActualReserva = EstadoReserva.Pendente,
                    Vendedor = novoVendedor
                },
                new Reservas
                {
                    DataInicio = new DateOnly(2023, 7, 1),
                    DataFim = new DateOnly(2023, 7, 31),
                    DataCriacao = DateTime.Now,
                    EstadoActualReserva = EstadoReserva.Pendente,
                    Vendedor = novoVendedor
                }
            );

            // Adiciona notificações
            context.Notificacoes.AddRange(
                new Notificacoes
                {
                    Conteudo = "Reserva aceite para o período de 1 a 15 de junho de 2023.",
                    DataCriacao = DateTime.Now,
                    EstadoActualNotificacao = EstadoNotificacao.Vista,
                    Vendedor = novoVendedor
                }
            );  

            context.SaveChanges();
        }
    }
}

