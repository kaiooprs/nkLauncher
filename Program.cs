using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using Spectre.Console;

namespace nkLauncher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ui = new InterfaceManager();
            var mine = new MinecraftManager();
            ui.MostrarCabecalho();

            bool rodando = true;
            while (rodando)
            {
                ui.LimparEHeader();
                
                string acao = ui.MenuPrincipal();

                if (acao == "Sair")
                {
                    rodando = false;
                }
                else if (acao == "Gerenciar Versões Locais")
                {
                    var locais = await mine.ObterVersoesLocaisAsync();
                    if (locais.Count == 0)
                        AnsiConsole.MarkupLine("[yellow]Nenhuma versão local encontrada.[/]");
                    else
                    {
                        string paraDeletar = ui.EscolherVersao("Escolha qual versão deseja [red]remover[/]:", locais);
                        if (ui.ConfirmarDelecao(paraDeletar))
                        {
                            mine.DeletarVersao(paraDeletar);
                            AnsiConsole.MarkupLine($"[green]Versão {paraDeletar} removida![/]");
                        }
                    }

                    AnsiConsole.MarkupLine("\n[grey]Pressione Enter para voltar ao menu...[/]");
                    Console.ReadLine();
                }
                else if (acao == "Iniciar Jogo")
                {
                    string tipoConta = ui.EscolherTipoConta();
                    string nick = "Jogador";
                    MSession sessaoAtual;

                    if (tipoConta == "Offline")
                    {
                        nick = ui.ObterNickname();
                        sessaoAtual = MSession.CreateOfflineSession(nick);
                    }
                    else
                    {
                        var authManager = new AuthManager();
                        try {
                            sessaoAtual = await authManager.AutenticarAsync();
                            nick = sessaoAtual.Username!;
                        } catch {
                            AnsiConsole.MarkupLine("[red]Login falhou! Caindo para modo Offline.[/]");
                            nick = ui.ObterNickname();
                            sessaoAtual = MSession.CreateOfflineSession(nick);
                        }
                    }

                    // --- LÓGICA DE VERSÕES ---
                    string origemVersao = ui.EscolherOrigemVersao();
                    string versaoEscolhida = "";

                    if (origemVersao.Contains("Vanilla"))
                    {
                        var vanillas = await mine.ObterVersoesVanillaAsync();
                        versaoEscolhida = ui.EscolherVersao("Escolha a versão:", vanillas);
                    }
                    else
                    {
                        var locais = await mine.ObterVersoesLocaisAsync();
                        if (locais.Count == 0) { AnsiConsole.MarkupLine("[red]Nenhuma versão local![/]"); continue; }
                        versaoEscolhida = ui.EscolherVersao("Escolha a versão local:", locais);
                    }

                    // --- INICIAR ---
                    int ramMb = ui.ConfigurarRam();
                    ui.MostrarResumo(nick, origemVersao, versaoEscolhida, ramMb);
                    
                    AnsiConsole.MarkupLine("\n[green]Pressione Enter para iniciar...[/]");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    await mine.IniciarJogoAsync(versaoEscolhida, ramMb, sessaoAtual);
                }
            }
            AnsiConsole.MarkupLine("[bold]Encerrando... Até mais![/]");
        }
    }
}