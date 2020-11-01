

<img src="https://user-images.githubusercontent.com/4818374/97811667-ce608b00-1c5a-11eb-85da-9c24ad25c081.png" height="128" width="128" /> 

 # Stream Bot

Projeto usando console aplication em C# usando o .Net Framework 4.7.2. Acompanhe a implementação na twitch em [daniel_dev](https://www.twitch.tv/daniel_dev)

Desenvolvi alguns recursos deste bot utilizando a biblioteca de integração com a twitch, chamada [TwitchLib](https://github.com/TwitchLib/TwitchLib), disponível no github e também como um pacote [Nuget TwitchLib](https://www.nuget.org/packages/TwitchLib).

### Outras bibliotecas usadas neste projeto
- [RestSharp](https://github.com/restsharp/RestSharp)
- [CsvHelper](https://github.com/JoshClose/CsvHelper)
- [JQuery](https://jquery.com/)
- [Dapper](https://github.com/StackExchange/Dapper)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
- [SignalR](https://github.com/SignalR/SignalR)

### Recursos implementados no bot:
- Execução de piadas usando uma base de dados SQLite já adicionada no projeto.
- Jogo de forca utilizando uma integração com SignalR para atualização em tempo real do jogo usando cliente Web.
- TTS para execução de falas baseadas em comentários.
- Execução de memes (audios) disponíveis no projeto.


### Até o momento temos a implementação do bot para
- Twitch

### Futuras implementações
- Associar imagens com os memes enviados pelos participantes.
- UI para controlar funções do bot.
- Parametrização de integrações.
- Integração com Streamlabs.
- Integração com YouTube.
- Integração com Facebook.
- Parametrização de recursos associados ao bot (Piada, Meme, TTS e etc) para utilização em outros serviços de stream.
