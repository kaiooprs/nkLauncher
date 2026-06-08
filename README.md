# 🚀 nkLauncher

Um launcher de Minecraft minimalista, direto ao ponto e totalmente focado em performance, rodando diretamente no seu terminal (CLI). Criado para quem quer abrir o jogo rápido sem interfaces pesadas rodando no fundo.

**⚠️ Nota Importante:** Este projeto é focado em uma experiência nativa para **Windows**.

---

## ✨ Funcionalidades

O nkLauncher foi construído para ser leve, mas não deixa nada a desejar em relação aos launchers grandes:

* **🔐 Login Original Seguro (Microsoft):** Sem gambiarras. Utiliza o motor oficial WebView2 do Windows. A janela de login que aparece é a da própria Microsoft, garantindo que suas credenciais vão direto para a fonte, com zero risco para a sua conta.
* **🏴‍☠️ Modo Offline Nativo:** Quer jogar na LAN com os amigos ou testar um mod offline? O launcher permite criar sessões locais com um nickname customizado.
* **💾 Persistência de Sessão:** Fez login uma vez? O launcher lembra de você. Na próxima abertura, você entra direto no menu, com seu nick já fixado no topo da tela.
* **📂 Gerenciamento de Versões Avançado:**
    * Lista e instala versões Vanilla diretamente dos servidores da Mojang.
    * Reconhece automaticamente versões locais com Modloaders (Forge, Fabric, NeoForge).
    * Menu interativo para deletar versões antigas e limpar seu HD diretamente pelo launcher.
* **⚙️ Alocação de RAM:** Escolha quanta memória o Minecraft pode usar antes de cada inicialização.
* **💨 Fire and Forget (Auto-Close):** Assim que o Minecraft abre, o launcher se encerra sozinho silenciosamente. Nada de terminais travados ou processos fantasmas roubando processamento do seu jogo.

---

## 🛠️ Como usar (Apenas Windows)

Não é necessário instalar Java, .NET ou qualquer dependência complexa. O executável já contém tudo o que precisa.

1. Vá até a aba **[Releases]** aqui no GitHub.
2. Baixe o arquivo `nkLauncher.exe`.
3. Coloque em uma pasta da sua preferência (ex: `C:\Games\nkLauncher`).
4. Dê um duplo clique e divirta-se!

---

## 🔒 Segurança e Privacidade

A segurança da sua conta é prioridade. O `nkLauncher` utiliza a biblioteca open-source [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core) para gerenciar o fluxo do jogo. 

Durante o login original, o launcher chama a API oficial da Microsoft (via Windows Forms / WebView2). **Em nenhum momento o seu e-mail ou senha passa pelo código do launcher**. O processo gera apenas um token de acesso temporário salvo localmente na sua máquina, seguindo os mesmos protocolos do Minecraft Launcher oficial.

---

## 💻 Para Desenvolvedores (Build)

Se você quiser compilar o código por conta própria:

1. Clone o repositório.
2. Certifique-se de ter o `.NET 10.0 SDK` (ou versão compatível) instalado.
3. No terminal da pasta do projeto, rode o comando abaixo para gerar um executável auto-contido com as bibliotecas nativas embutidas:

dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
