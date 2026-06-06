using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CmlLib.Core;
using CmlLib.Core.Auth;         
using CmlLib.Core.ProcessBuilder; 
using Spectre.Console;         

namespace nkLauncher
{
    public class MinecraftManager
    {
        private readonly MinecraftLauncher _launcher;

        public MinecraftManager()
        {
            _launcher = new MinecraftLauncher();
        }

        public async Task<List<string>> ObterVersoesVanillaAsync()
        {
            var todasVersoes = await _launcher.GetAllVersionsAsync();
            var listaFiltrada = new List<string>();
        
            foreach (var v in todasVersoes.Where(v => v.Type == "release"))
            {
                listaFiltrada.Add(v.Name);
                
                if (v.Name == "1.7.10") 
                    break; 
            }
        
            return listaFiltrada;
        }

        public async Task<List<string>> ObterVersoesLocaisAsync()
        {
            var todasVersoes = await _launcher.GetAllVersionsAsync();
            return todasVersoes
                .Where(v => File.Exists(Path.Combine(_launcher.MinecraftPath.Versions, v.Name, $"{v.Name}.json")))
                .Select(v => v.Name)
                .ToList();
        }

        public async Task IniciarJogoAsync(string versao, int ramMb, MSession session)
        {
            var launchOptions = new MLaunchOption
            {
                Session = session, 
                MaximumRamMb = ramMb
            };

            await AnsiConsole.Progress()
                .Columns(new ProgressColumn[] 
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(), 
                    new PercentageColumn(),  
                    new SpinnerColumn()      
                })
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask($"[green]Verificando arquivos da {versao}...[/]");

                    _launcher.FileProgressChanged += (sender, args) =>
                    {
                        task.MaxValue = args.TotalTasks;
                        task.Value = args.ProgressedTasks;
                        task.Description = $"[blue]Baixando/Verificando:[/] {args.ProgressedTasks} de {args.TotalTasks}";
                    };

                    var process = await _launcher.InstallAndBuildProcessAsync(versao, launchOptions);

                    task.Value = task.MaxValue;
                    task.Description = "[green]Tudo pronto! Iniciando a JVM...[/]";
                    
                    process.Start();
                });
        }

        public void DeletarVersao(string nomeVersao)
        {
            string path = Path.Combine(_launcher.MinecraftPath.Versions, nomeVersao);
            
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}