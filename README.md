# Serviço de Find.Collect

Esta aplicação tem como objetivo facilitar a vida de colecionadores de carrinhos em miniatura ao fornecer uma plataforma centralizada para localizar lojas especializadas nesse tipo de item. Desenvolvido com foco na organização e escalabilidade, o sistema adota uma arquitetura em camadas baseada nos princípios do Domain-Driven Design (DDD), garantindo uma separação clara de responsabilidades entre as camadas de domínio, serviço, infraestrutura e Api.

A API fornece endpoints RESTful para cadastro, consulta e atualização de lojas, marcas e produtos voltados ao nicho de miniaturas, permitindo que o usuário encontre pontos de venda conforme localização, marcas disponíveis e categorias de interesse.

O projeto utiliza tecnologias modernas como .NET 9.0, Entity Framework Core, Docker e segue boas práticas de desenvolvimento, sendo ideal tanto para ambientes locais de desenvolvimento quanto para futura implantação em produção.

Os primeiros passos são criar um usuário(via swagger ou frontend) para ter acesso á todos os fluxos.

---

## Requisitos mínimos para desenvolvimento

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
  
- Uma IDE de desenvolvimento em C# / .NET (recomendações: [Visual Studio](https://visualstudio.microsoft.com/pt-br/vs/) ou [Visual Studio Code](https://code.visualstudio.com/))
  
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
  
- Pode ser usado o Docker via [WSL](https://docs.docker.com/desktop/features/wsl/)

---

## Como executar a aplicação localmente?

* No diretório docker execute o comando docker-compose up -d (para criar/subir o container do PostgreSQL)
* Execute o projeto Api com o perfil https da aplicação na IDE
* Acesse o [Swagger da API](https://localhost:7240/swagger/index.html)
