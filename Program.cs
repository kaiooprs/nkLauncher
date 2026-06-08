using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using Spectre.Console;

namespace nkLauncher
{
    class Program
    {
        private const string SessionFile = "last_session.txt";

        static async Task Main(string[] args)
        {
            var ui = new InterfaceManager();
            var mine = new MinecraftManager();

            string nick = "Jogador";
            MSession sessaoAtual = null!;
            bool logadoComSucesso = false;

            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("nkLauncher").LeftJustified().Color(Color.Green));
            AnsiConsole.MarkupLine("[grey]Verificando sessões salvas...[/]\n");

            if (File.Exists(SessionFile))
            {
                try
                {
                    var dados = await File.ReadAllLinesAsync(SessionFile);
                    if (dados.Length >= 2)
                    {
                        string tipoContaSalva = dados[0];
                        string nickSalvo = dados[1];

                        if (tipoContaSalva == "Offline")
                        {
                            nick = nickSalvo;
                            sessaoAtual = MSession.CreateOfflineSession(nick);
                            logadoComSucesso = true;
                        }
                        else if (tipoContaSalva == "Online")
                        {
                            var authManager = new AuthManager();
                            sessaoAtual = await authManager.AutenticarAsync();
                            nick = sessaoAtual.Username!;
                            logadoComSucesso = true;
                        }
                    }
                }
                catch
                {
                    logadoComSucesso = false;
                }
            }

            if (!logadoComSucesso)
            {
                string tipoConta = ui.EscolherTipoConta();

                if (tipoConta == "Offline")
                {
                    nick = ui.ObterNickname();
                    sessaoAtual = MSession.CreateOfflineSession(nick);
                    // Salva para a próxima vez
                    await File.WriteAllLinesAsync(SessionFile, new[] { "Offline", nick });
                }
                else
                {
                    var authManager = new AuthManager();
                    try
                    {
                        sessaoAtual = await authManager.AutenticarAsync();
                        nick = sessaoAtual.Username!;
                        await File.WriteAllLinesAsync(SessionFile, new[] { "Online", nick });
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"\n[red]Erro no login automático: {ex.Message}[/]");
                        AnsiConsole.MarkupLine("[yellow]Alternando para modo Offline por segurança...[/]");
                        
                        nick = ui.ObterNickname();
                        sessaoAtual = MSession.CreateOfflineSession(nick);
                        await File.WriteAllLinesAsync(SessionFile, new[] { "Offline", nick });
                    }
                }
            }

            bool rodando = true;
            while (rodando)
            {
                ui.LimparEHeader(nick);

                string acao = ui.MenuPrincipal();

                if (acao == "Sair")
                {
                    rodando = false;
                }
                else if (acao == "Gerenciar Versões Locais")
                {
                    var locais = await mine.ObterVersoesLocaisAsync();
                    
                    if (locais.Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Nenhuma versão local encontrada para gerenciar.[/]");
                        AnsiConsole.MarkupLine("\n[grey]Pressione Enter para voltar...[/]");
                        Console.ReadLine();
                        continue;
                    }

                    var opcoesMenu = new List<string>(locais);
                    string opcaoSair = "[yellow]<- Voltar (Não fazer nada)[/]";
                    opcoesMenu.Add(opcaoSair);

                    string escolha = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Selecione uma versão para gerenciar ou escolha voltar:")
                            .PageSize(10)
                            .AddChoices(opcoesMenu));

                    if (escolha == opcaoSair) continue;

                    string acaoVersao = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"O que você deseja fazer com a versão [blue]{escolha}[/]?")
                            .AddChoices("Apagar Versão", "Voltar"));

                    if (acaoVersao == "Apagar Versão")
                    {
                        if (ui.ConfirmarDelecao(escolha))
                        {
                            mine.DeletarVersao(escolha);
                            AnsiConsole.MarkupLine($"\n[green]Sucesso: A versão {escolha} foi apagada![/]");
                            AnsiConsole.MarkupLine("\n[grey]Pressione Enter para continuar...[/]");
                            Console.ReadLine();
                        }
                    }
                }
                else if (acao == "Iniciar Jogo")
                {
                    string origemVersao = ui.EscolherOrigemVersao();
                    string versaoEscolhida = "";

                    if (origemVersao.Contains("Vanilla"))
                    {
                        var vanillas = await mine.ObterVersoesVanillaAsync();
                        versaoEscolhida = ui.EscolherVersao("Escolha a versão [green]Vanilla[/]:", vanillas);
                    }
                    else
                    {
                        var locais = await mine.ObterVersoesLocaisAsync();
                        if (locais.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]Nenhuma versão local encontrada![/]");
                            AnsiConsole.MarkupLine("\n[grey]Pressione Enter para continuar...[/]");
                            Console.ReadLine();
                            continue;
                        }
                        versaoEscolhida = ui.EscolherVersao("Escolha uma versão [blue]Local[/]:", locais);
                    }

                    int ramMb = ui.ConfigurarRam();
                    ui.MostrarResumo(nick, origemVersao, versaoEscolhida, ramMb);
                    
                    AnsiConsole.MarkupLine("\n[green]Pressione Enter para iniciar o Minecraft...[/]");
                    Console.ReadLine();
                    AnsiConsole.Clear();

                    AnsiConsole.MarkupLine("[grey]Iniciando o jogo... O launcher será fechado automaticamente.[/]");
                    
                    await mine.IniciarJogoAsync(versaoEscolhida, ramMb, sessaoAtual);

                    await Task.Delay(2000); 
                    Environment.Exit(0);
                }
            }
            AnsiConsole.MarkupLine("[bold]Encerrando o launcher. Até mais![/]");
        }
    }
}