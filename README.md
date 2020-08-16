# GHDER - GitHub Downloader

## Intro

This repository contains a programming challenge for the job of FullStack Developer.

It's a API that returns the quantity of lines of the files in a GitHub repository.

## Tools

- Visual Studio 2019
- Asp.Net Mvc Core
- C#
- Xunit for tests
- HtmlAgilityPack for Html parsing
- Swagger for API documentation
- Docker
- Heroku for Hosting (https://ghder.herokuapp.com/)
- Docker Hub for the Image registry (https://hub.docker.com/repository/docker/diegobarbosa/ghder)


## API EndPoint

https://ghder.herokuapp.com/

By Default, this address will return the Swagger documentation page. Is possible to execute (test) the API in this docs page.

Calling the api in the browser: https://ghder.herokuapp.com/service/{userName}/{repoName}

Example: https://ghder.herokuapp.com/service/diegobarbosa/ghder


## Running the docker image localy
Execute the following commands in your CMD:

```console
docker pull diegobarbosa/ghder

docker run -d -p 8080:80 --name ghder diegobarbosa/ghder
```

