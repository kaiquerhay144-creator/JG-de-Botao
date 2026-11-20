# JG de Botão

Protótipo inicial de um jogo de futebol de botão 3D para Android.

## Visão do projeto
- Campo 3D com gramado, linhas, gols, bola e 22 botões.
- Controles por toque: arraste a partir de um botão para definir direção e força.
- Identidade visual inicial própria, com publicidade RHAY.
- Próximas etapas: regras completas, times configuráveis, menus, partidas online, salas privadas, ranking e campeonatos.

## Tecnologia
Projeto-fonte preparado para Unity 3D (C#). O APK Android precisa ser compilado no Unity Editor com Android Build Support instalado. Este repositório ainda não contém um APK compilado nem servidor multiplayer.

## Como abrir
1. Instale o Unity Hub em um computador.
2. Crie/abra um projeto Unity 3D (Built-in Render Pipeline).
3. Copie os arquivos de `Assets/Scripts` para a pasta correspondente do projeto.
4. Crie uma cena vazia e adicione um GameObject chamado `Game`.
5. Anexe o script `JGGameBootstrap` ao GameObject e execute a cena.

O protótipo monta o campo e os jogadores por código, sem precisar importar modelos externos.
