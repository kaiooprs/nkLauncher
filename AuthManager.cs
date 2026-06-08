using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Spectre.Console;

namespace nkLauncher
{
    public class AuthManager
    {
        private readonly JELoginHandler _loginHandler;

        public AuthManager()
        {
            _loginHandler = JELoginHandlerBuilder.BuildDefault();
        }

        public async Task<MSession> AutenticarAsync()
        {
            try
            {
                AnsiConsole.MarkupLine("\n[grey]Verificando credenciais da Microsoft...[/]");
                AnsiConsole.MarkupLine("[grey](Se necessário, uma janela de login será aberta)[/]");

                var session = await _loginHandler.Authenticate();
                
                AnsiConsole.MarkupLine($"\n[bold green]Login concluído com sucesso! Bem-vindo, {session.Username}![/]");
                return session;
            }
            catch (Exception ex)
            {
                throw new Exception($"Operação cancelada ou falha na autenticação: {ex.Message}");
            }
        }
    }
}