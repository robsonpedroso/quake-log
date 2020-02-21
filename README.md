# Quake Log Parser

Projeto para leitura de arquivo de log do jogo Quake

## Introdu��o

Essas instru��es fornecer�o uma c�pia do projeto em execu��o na sua m�quina local para fins de desenvolvimento e teste.

Aplica��o foi baseada na documenta��o [Quake log parser](https://gist.github.com/sergiofillipi/d049634d4b7d0cb01812322de51c6239)

## Alguns pontos importantes 

Infelizmente, ao meu ponto de vista, o documento n�o esta muito bem especificado, com isso n�o entendi alguns pontos:

1. Se era pra fazer um `service` para gerar um arquivo de parser
2. O Rank, n�o deixei ser negativo, caso o mesmo seja `0` ele n�o ira subtrair 1 kill do player quando o `world` matar o player
3. O formato do parser n�o � um formato de json v�lido e nem o de agrupamento do relat�rio de mortes
4. Um script que imprima um relat�rio (???) essa eu sinceramente n�o entendi e fiz atr�ves de API pois achei mais sensato e coerente, se n�o for isso, especifique um pouco melhor para que eu ajuste a API ou um projeto em Angular
5. Estava informando que era pra ser feito em JAVA, como n�o tenho (muito) conhecimento em JAVA fiz em C# dotnet Core 2.2 pois � minha especialidade.
6. Vi que o log tem alguns `ClientDisconnect` e posteriormente o usu�rio (em alguns casos) conecta novamente com o mesmo nome, por�m com outro ID. Achei melhor tratar como novo player, pois seria um erro diferenciar os players pelo nome.
7. Quando for o proprio jogador que se matou, � pra contabilizar o kill de alguma forma? Atualmente esta contabilizando como positivo, porem pra n�o contabilizar ou contabilizar negativo, basta validar o id do killer com o id do killed

Para ler o log e fazer o parse para o meu DTO, optei pelo Regex (sinceramente n�o sou muito f�), pois assim fica mais f�cil e leg�vel trabalhar do que fazer split, replace ou substring em cada propriedade para varrer as linhas.

### Prerequisitos

O que voc� precisa para baixar, rodar e disponibilizar.

* Dotnet core 3.1 instalado

### Instala��o

Ap�s a execu��o do pre requisitos, segue um passo a passo de como rodar localmente.

Clonar o reposit�rio

```
git clone git@github.com:robsonpedroso/quake-log.git
```

Abra a solu��o com o Visual Studio e compile.
- Pode ser feito pelo bash, terminal ou cmd atrav�s do comando `dotnet build`

Sete o Projeto default como a API e execute (F5).

Chame a URL abaixo pelo navegador para verificar se esta ok.

```
GET /api/v1/ping
```

Se ele retornar ok (conforme exemplo abaixo), informa que n�o ouve erro na aplica��o.

```
{
    "content": "pong",
    "status": "OK",
    "messages": []
}
```

### Estrutura de pastas de solu��o

1. `_docs` - Contem o arquivo Readme.md e caso necess�rio outras documenta��es para suporte a execu��o e manuten��o da aplica��o.
2. `api` - Projeto da API
3. `core` - Estrutura padr�o do DDD contendo os projetos `Application`, `Domain` e `Infra`
4. `tools` - Ferramentas para ajudar no desenvolvimento, no caso foi usado algumas extensions para facilitar a implementa��o da API e dos retornos.
5. `tests` - Projeto de testes utilizando o [Moq4](https://github.com/moq/moq4)

### Padr�o de Tecnologia utilizado

Na empresa onde trabalho atualmente utilizamos esse padr�o do DDD mais simplificado para trabalhar com os projetos.

Os contratos n�o s�o utilizados no projeto de `Application` devido ser 1 por 1, caso haja algum caso que seja necess�rio fazer uma diferencia��o de aplica��o, ai sim criamos a interface para essa diferencia��o, sinceramente isso nunca aconteceu nos nosso projetos por esse motivo fazemos dessa forma

J� no `Domain` e no `Infra` utilizamos normalmente os contratos (interfaces), pois sabemos que muitas das vezes precisamos modificar os servi�os, seja por causa de alguma integra��o ou ferramenta utilizada que foi necess�rio mudar o padr�o de conex�es e chamadas entre elas.

O mapeamento das interfaces s�o feito automaticamente com reflection para evitar o trabalho e poss�veis erros de esquecimento.
Esse reflection se encontra numa extension no projeto `tools/WebApi` e � chamado no startup da aplica��o.
Caso seja necess�rio passar alguma interface externa ou manualmente mesmo, esse metodo aceita um action ficando mais f�cil utilizar.

Exemplo da utiliza��o se encontra no Statup (veja abaixo):
```
	services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, LogProcessInfraServices>(srv =>
    {
        srv.AddSingleton<Config>();
    });
```

O retorno da API foi modificado atrav�s de um wrapper e filtro no startup da API.
O padr�o de convers�o do json � `SnakeCaseNamingStrategy`.
Para facilitar a visualiza��o do json de resultado utilizei o [Json Viewer Online](http://jsonviewer.stack.hu/)

### Configura��es no AppSettings da API

Existem algumas configura��es no settings da API, para ajudar tanto no desenvolvimento quanto na manuten��o, pois assim, para essas configura��es n�o se faz necess�rio o build e subida de pacote (dlls).

```
  "Game": {
    "BasePath": "C:\\dev\\QuakeLog",
    "LogPath": "assets",
    "LogName": "games.log",
    "World": 1022,
    "AllActions": "(InitGame|ClientConnect|ClientUserinfoChanged|Kill)"
  }
```

Esse arquivo de settings � gerenciado no `Domain/Config.cs` e carregado na inje��o de depend�ncias no startup para receber em qualquer lugar atr�ves do construtor.

Descri��o das configura��es
1. `BasePath` - Diret�rio onde se encontra a aplica��o
2. `LogPath` - [Apartir do diret�rio base] Esse esse diret�rio informa onde fica o arquivo de log
3. `LogName` - Nome do arquivo de log
4. `World` - Id do world do log que n�o pode ser contabilizado como player
5. `AllActions` - A��es do log que s�o utilizadas para o carregamento do log e parse para o DTO de Games

Se a chave `BasePath` n�o for configurada, a aplica��o pega o diret�rio corrente na vari�vel de ambiente.
APENAS para facilitar o teste da aplica��o, inclu� o log dentro da pasta `assets` no diret�rio da API e n�o preenchi a chave `BasePath`.
Com isso basta executar a aplica��o para come�ar os testes.

## Gerar imagem em Docker

Criei o `Dockerfile` para facilitar a cria��o da imagem para subida em container docker

## Subir numa maquina virtual

Criei o arquivo Vagrantfile para facilitar a subida da maquina virtual, segue o passo a passo que � executado.

### Prerequisitos

* Vagrant
* Virtual Box

### Executando o Vagrant

Antes de mais nada � preciso instalar o plugin abaixo pois o Vagrant vai precisar compartilhar pastas com a maquina local para ler os arquivos do projeto

`vagrant plugin install vagrant-vbguest`

Subindo a maquina virtual:

`vagrant up`

Segue o passo a passo dessa execu��o:

- Subir a maquina virutal com o `Ubuntu 19` rodando
- Instalar o `Docker`
- Gerar a imagem do docker apontando para o arquivo `Dockerfile`
- Gerar um container expondo a porta `80`



## Execu��o dos Endpoints

```
GET /api/v1/games/task-one
```

Nesse endpoint � retornado todos os registros j� excluso o `world` dos kills e players, mas mantendo a soma no total e no agrupamento de motivos de mortes, se por armas ou outro - `kills_means`

Exemplo de retorno:

```
{
  "content": {
    "id": 3,
    "total_kills": 4,
    "players": [
      {
        "id": 2,
        "name": "Dono da Bola",
        "rank": 0
      },
      {
        "id": 3,
        "name": "Isgalamido",
        "rank": 1
      },
      {
        "id": 4,
        "name": "Zeh",
        "rank": 0
      }
    ],
    "kills": [
      {
        "killer": {
          "id": 3,
          "name": "Isgalamido",
          "rank": 1
        },
        "killed": {
          "id": 2,
          "name": "Dono da Bola",
          "rank": 0
        },
        "means_of_death": 6,
        "description_means_of_death": "MOD_ROCKET"
      }
    ],
    "kills_means": [
      {
        "count": 1,
        "means_of_death": 6,
        "description_means_of_death": "MOD_ROCKET"
      },
      {
        "count": 2,
        "means_of_death": 22,
        "description_means_of_death": "MOD_TRIGGER_HURT"
      },
      {
        "count": 1,
        "means_of_death": 19,
        "description_means_of_death": "MOD_FALLING"
      }
    ]
  },
  "status": "OK",
  "messages": [
    
  ]
}
```

### GET /api/v1/games/all

```
GET /api/v1/games/all
```

Nesse endpoint � retornado todos os registros encontrados no log referente as partidas, jogos e mortes incluindo as mortes causadas pelo `world`.

Exemplo de retorno:

```
{
    "content": [
		{
		  "id": 3,
		  "total_kills": 4,
		  "players": [
			{
			  "id": 2,
			  "name": "Dono da Bola",
			  "rank": 0
			},
			{
			  "id": 3,
			  "name": "Isgalamido",
			  "rank": 1
			},
			{
			  "id": 4,
			  "name": "Zeh",
			  "rank": 0
			}
		  ],
		  "kills": [
			{
			  "killer": {
				"id": 3,
				"name": "Isgalamido",
				"rank": 1
			  },
			  "killed": {
				"id": 2,
				"name": "Dono da Bola",
				"rank": 0
			  },
			  "means_of_death": 6,
			  "description_means_of_death": "MOD_ROCKET"
			},
			{
			  "killer": {
				"id": 1022,
				"name": "World",
				"rank": 0
			  },
			  "killed": {
				"id": 4,
				"name": "Zeh",
				"rank": 0
			  },
			  "means_of_death": 22,
			  "description_means_of_death": "MOD_TRIGGER_HURT"
			},
			{
			  "killer": {
				"id": 1022,
				"name": "World",
				"rank": 0
			  },
			  "killed": {
				"id": 4,
				"name": "Zeh",
				"rank": 0
			  },
			  "means_of_death": 22,
			  "description_means_of_death": "MOD_TRIGGER_HURT"
			},
			{
			  "killer": {
				"id": 1022,
				"name": "World",
				"rank": 0
			  },
			  "killed": {
				"id": 2,
				"name": "Dono da Bola",
				"rank": 0
			  },
			  "means_of_death": 19,
			  "description_means_of_death": "MOD_FALLING"
			}
		  ],
		  "kills_means": [
			{
			  "count": 1,
			  "means_of_death": 6,
			  "description_means_of_death": "MOD_ROCKET"
			},
			{
			  "count": 2,
			  "means_of_death": 22,
			  "description_means_of_death": "MOD_TRIGGER_HURT"
			},
			{
			  "count": 1,
			  "means_of_death": 19,
			  "description_means_of_death": "MOD_FALLING"
			}
		  ]
		}
	],
    "status": "OK",
    "messages": []
}
```


### GET /api/v1/games/{codigo-da-partida:inteiro}

```
GET /api/v1/games/{codigo-da-partida:inteiro}
```

Nesse endpoint � retornado apenas a partida informada, j� excluso o `world` dos kills e players, mas mantendo a soma no total e no agrupamento de motivos de mortes, se por armas ou outro - `kills_means`
O retorno � o mesmo padr�o do json do entpoint `/api/v1/games/all` informado acima.


## Execu��o dos testes

1. Abra o Visual Studio
2. Menu Test > Run > All Tests

Pelo bash, terminal ou cmd
`dotnet test`

## Publica��o

N�o foi publicado

## Autores

* **Robson Pedroso** - *Projeto inicial* - [RobsonPedroso](https://github.com/robsonpedroso)

## License

Software feito apenas para fins de estudo

## Biblioteca

[Moq4](https://github.com/moq/moq4)