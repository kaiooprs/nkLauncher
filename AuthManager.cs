using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.Game.Msal;
using Spectre.Console;

namespace nkLauncher
{
    public class AuthManager
    {
        private readonly string _clientId; 
        private readonly JELoginHandler _loginHandler;

        public AuthManager()
        {
            try
            {
                _clientId = System.IO.File.ReadAllText("client_id.txt").Trim();
            }
            catch
            {
                AnsiConsole.MarkupLine("[red]Erro: Arquivo 'client_id.txt' não encontrado na pasta do projeto![/]");
                Environment.Exit(1);
            }
            _loginHandler = JELoginHandlerBuilder.BuildDefault();
        }

        public async Task<MSession> AutenticarAsync()
        {
            try
            {
                AnsiConsole.MarkupLine("\n[grey]Procurando sessão salva da Microsoft...[/]");
                
                var session = await _loginHandler.AuthenticateSilently();
                
                AnsiConsole.MarkupLine($"[green]Sessão restaurada! Bem-vindo de volta, {session.Username}![/]");
                return session;
            }
            catch (Exception)
            {
                AnsiConsole.MarkupLine("[yellow]Nenhuma sessão válida. Iniciando comunicação com o Xbox...[/]");

                var app = await MsalClientHelper.BuildApplicationWithCache(_clientId);
                var authenticator = _loginHandler.CreateAuthenticatorWithNewAccount();

                // --- ESCOLHA O MÉTODO DE LOGIN AQUI ---

                // MÉTODO 1 (Comentado): Browser Automático (Usar este quando o seu ClientID for aprovado)
                authenticator.AddMsalOAuth(app, msal => msal.SystemBrowser());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddJEAuthenticator();

                AnsiConsole.Markup("\n[cyan]Aguardando autorização... (Siga as instruções acima)[/]");

                var session = await authenticator.ExecuteForLauncherAsync();
                
                AnsiConsole.MarkupLine($"\n\n[bold green]Login original concluído! Bem-vindo, {session.Username}![/]");
                return session;
            }
        }
    }
}