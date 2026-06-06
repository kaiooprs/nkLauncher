using System.Collections.Generic;
using Spectre.Console;

namespace nkLauncher
{
    public class InterfaceManager
    {
        public void MostrarCabecalho()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("nkLauncher").LeftJustified().Color(Color.Green));
            AnsiConsole.MarkupLine("[grey]O launcher minimalista e direto ao ponto.[/]\n");
        }

        public string EscolherTipoConta()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Como você deseja [green]entrar[/]?")
                    .AddChoices("Offline", "Original (Microsoft)"));
        }

        public string ObterNickname()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Digite seu [blue]nickname[/]:")
                    .DefaultValue("Fulano")
                    .AllowEmpty());
        }

        public string EscolherOrigemVersao()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nO que você deseja [green]jogar[/]?")
                    .AddChoices("Vanilla (Catálogo da Mojang)", "Instalações Locais (Fabric, Forge, etc.)"));
        }

        public string EscolherVersao(string titulo, List<string> versoes)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(titulo)
                    .PageSize(10)
                    .AddChoices(versoes));
        }

        public int ConfigurarRam()
        {
            if (!AnsiConsole.Confirm("\nDeseja alterar a quantidade de [yellow]RAM[/]? (Padrão: 4GB)", defaultValue: false))
                return 4096;

            var gb = AnsiConsole.Prompt(
                new TextPrompt<int>("Quantos [yellow]GB[/] deseja alocar?")
                    .DefaultValue(4)
                    .ValidationErrorMessage("[red]Digite um número válido![/]")
                    .Validate(ram => ram > 0 && ram <= 32 ? ValidationResult.Success() : ValidationResult.Error("[red]Valor de RAM inválido.[/]")));

            return gb * 1024;
        }

        public string MenuPrincipal()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("O que vamos [green]fazer[/] hoje?")
                    .AddChoices("Iniciar Jogo", "Gerenciar Versões Locais", "Sair"));
        }
        
        public bool ConfirmarDelecao(string versao)
        {
            return AnsiConsole.Confirm($"Você tem certeza que deseja deletar a versão [red]{versao}[/]?");
        }

        public void LimparEHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("nkLauncher").LeftJustified().Color(Color.Green));
            AnsiConsole.MarkupLine("[grey]O launcher minimalista e direto ao ponto.[/]\n");
        }

        public void MostrarResumo(string nick, string origem, string versao, int ramMb)
        {
            AnsiConsole.Clear();
            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("[green]Configuração[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("Nickname", nick);
            table.AddRow("Origem", origem.Contains("Vanilla") ? "Oficial" : "Local");
            table.AddRow("Versão", versao);
            table.AddRow("RAM", $"{ramMb / 1024}GB");

            AnsiConsole.Write(table);
        }
    }
}