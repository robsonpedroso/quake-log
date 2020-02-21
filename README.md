# Quake Log Parser

Projeto para leitura de arquivo de log do jogo Quake

## Introdução

Essas instruções fornecerão uma cópia do projeto em execução na sua máquina local para fins de desenvolvimento e teste.

Aplicação foi baseada na documentação [Quake log parser](https://gist.github.com/sergiofillipi/d049634d4b7d0cb01812322de51c6239)

## Alguns pontos importantes 

Infelizmente, ao meu ponto de vista, o documento não esta muito bem especificado, com isso não entendi alguns pontos:

1. Se era pra fazer um `service` para gerar um arquivo de parser
2. O Rank, não deixei ser negativo, caso o mesmo seja `0` ele não ira subtrair 1 kill do player quando o `world` matar o player
3. O formato do parser não é um formato de json válido e nem o de agrupamento do relatório de mortes
4. Um script que imprima um relatório (???) essa eu sinceramente não entendi e fiz atráves de API pois achei mais sensato e coerente, se não for isso, especifique um pouco melhor para que eu ajuste a API ou um projeto em Angular
5. Estava informando que era pra ser feito em JAVA, como não tenho (muito) conhecimento em JAVA fiz em C# dotnet Core 2.2 pois é minha especialidade.
6. Vi que o log tem alguns `ClientDisconnect` e posteriormente o usuário (em alguns casos) conecta novamente com o mesmo nome, porém com outro ID. Achei melhor tratar como novo player, pois seria um erro diferenciar os players pelo nome.
7. Quando for o proprio jogador que se matou, é pra contabilizar o kill de alguma forma? Atualmente esta contabilizando como positivo, porem pra não contabilizar ou contabilizar negativo, basta validar o id do killer com o id do killed

Para ler o log e fazer o parse para o meu DTO, optei pelo Regex (sinceramente não sou muito fã), pois assim fica mais fácil e legível trabalhar do que fazer split, replace ou substring em cada propriedade para varrer as linhas.

### Prerequisitos

O que você precisa para baixar, rodar e disponibilizar.

* Dotnet core 3.1 instalado

### Instalação

Após a execução do pre requisitos, segue um passo a passo de como rodar localmente.

Clonar o repositório

```
git clone git@github.com:robsonpedroso/quake-log.git
```

Abra a solução com o Visual Studio e compile.
- Pode ser feito pelo bash, terminal ou cmd através do comando `dotnet build`

Sete o Projeto default como a API e execute (F5).

Chame a URL abaixo pelo navegador para verificar se esta ok.

```
GET /api/v1/ping
```

Se ele retornar ok (conforme exemplo abaixo), informa que não ouve erro na aplicação.

```
{
    "content": "pong",
    "status": "OK",
    "messages": []
}
```

### Estrutura de pastas de solução

1. `_docs` - Contem o arquivo Readme.md e caso necessário outras documentações para suporte a execução e manutenção da aplicação.
2. `api` - Projeto da API
3. `core` - Estrutura padrão do DDD contendo os projetos `Application`, `Domain` e `Infra`
4. `tools` - Ferramentas para ajudar no desenvolvimento, no caso foi usado algumas extensions para facilitar a implementação da API e dos retornos.
5. `tests` - Projeto de testes utilizando o [Moq4](https://github.com/moq/moq4)

### Padrão de Tecnologia utilizado

Na empresa onde trabalho atualmente utilizamos esse padrão do DDD mais simplificado para trabalhar com os projetos.

Os contratos não são utilizados no projeto de `Application` devido ser 1 por 1, caso haja algum caso que seja necessário fazer uma diferenciação de aplicação, ai sim criamos a interface para essa diferenciação, sinceramente isso nunca aconteceu nos nosso projetos por esse motivo fazemos dessa forma

Já no `Domain` e no `Infra` utilizamos normalmente os contratos (interfaces), pois sabemos que muitas das vezes precisamos modificar os serviços, seja por causa de alguma integração ou ferramenta utilizada que foi necessário mudar o padrão de conexões e chamadas entre elas.

O mapeamento das interfaces são feito automaticamente com reflection para evitar o trabalho e possíveis erros de esquecimento.
Esse reflection se encontra numa extension no projeto `tools/WebApi` e é chamado no startup da aplicação.
Caso seja necessário passar alguma interface externa ou manualmente mesmo, esse metodo aceita um action ficando mais fácil utilizar.

Exemplo da utilização se encontra no Statup (veja abaixo):
```
	services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, LogProcessInfraServices>(srv =>
    {
        srv.AddSingleton<Config>();
    });
```

O retorno da API foi modificado através de um wrapper e filtro no startup da API.
O padrão de conversão do json é `SnakeCaseNamingStrategy`.
Para facilitar a visualização do json de resultado utilizei o [Json Viewer Online](http://jsonviewer.stack.hu/)

### Configurações no AppSettings da API

Existem algumas configurações no settings da API, para ajudar tanto no desenvolvimento quanto na manutenção, pois assim, para essas configurações não se faz necessário o build e subida de pacote (dlls).

```
  "Game": {
    "BasePath": "C:\\dev\\QuakeLog",
    "LogPath": "assets",
    "LogName": "games.log",
    "World": 1022,
    "AllActions": "(InitGame|ClientConnect|ClientUserinfoChanged|Kill)"
  }
```

Esse arquivo de settings é gerenciado no `Domain/Config.cs` e carregado na injeção de dependências no startup para receber em qualquer lugar atráves do construtor.

Descrição das configurações
1. `BasePath` - Diretório onde se encontra a aplicação
2. `LogPath` - [Apartir do diretório base] Esse esse diretório informa onde fica o arquivo de log
3. `LogName` - Nome do arquivo de log
4. `World` - Id do world do log que não pode ser contabilizado como player
5. `AllActions` - Ações do log que são utilizadas para o carregamento do log e parse para o DTO de Games

Se a chave `BasePath` não for configurada, a aplicação pega o diretório corrente na variável de ambiente.
APENAS para facilitar o teste da aplicação, incluí o log dentro da pasta `assets` no diretório da API e não preenchi a chave `BasePath`.
Com isso basta executar a aplicação para começar os testes.

## Gerar imagem em Docker

Criei o `Dockerfile` para facilitar a criação da imagem para subida em container docker

## Subir numa maquina virtual

Criei o arquivo Vagrantfile para facilitar a subida da maquina virtual, segue o passo a passo que é executado.

### Prerequisitos

* Vagrant
* Virtual Box

### Executando o Vagrant

Antes de mais nada é preciso instalar o plugin abaixo pois o Vagrant vai precisar compartilhar pastas com a maquina local para ler os arquivos do projeto

`vagrant plugin install vagrant-vbguest`

Subindo a maquina virtual:

`vagrant up`

Segue o passo a passo dessa execução:

- Subir a maquina virutal com o `Ubuntu 19` rodando
- Instalar o `Docker`
- Gerar a imagem do docker apontando para o arquivo `Dockerfile`
- Gerar um container expondo a porta `80`



## Execução dos Endpoints

```
GET /api/v1/games/task-one
```

Nesse endpoint é retornado todos os registros já excluso o `world` dos kills e players, mas mantendo a soma no total e no agrupamento de motivos de mortes, se por armas ou outro - `kills_means`

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

Nesse endpoint é retornado todos os registros encontrados no log referente as partidas, jogos e mortes incluindo as mortes causadas pelo `world`.

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

Nesse endpoint é retornado apenas a partida informada, já excluso o `world` dos kills e players, mas mantendo a soma no total e no agrupamento de motivos de mortes, se por armas ou outro - `kills_means`
O retorno é o mesmo padrão do json do entpoint `/api/v1/games/all` informado acima.


## Execução dos testes

1. Abra o Visual Studio
2. Menu Test > Run > All Tests

Pelo bash, terminal ou cmd
`dotnet test`

## Publicação

Não foi publicado

## Autores

* **Robson Pedroso** - *Projeto inicial* - [RobsonPedroso](https://github.com/robsonpedroso)

## License

Software feito apenas para fins de estudo

## Biblioteca

[Moq4](https://github.com/moq/moq4)