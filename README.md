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
- Docker - Each time the image is Built, the Test suite is executed
- Heroku for Hosting (https://ghder.herokuapp.com/)
- Docker Hub for the Image registry (https://hub.docker.com/repository/docker/diegobarbosa/ghder)
- AppVeyor for CI - Each new Push to GitHub triggers a Build and executes the Tests suite: [![Build status](https://ci.appveyor.com/api/projects/status/e0gus2bb7iug9i74/branch/master?svg=true)](https://ci.appveyor.com/project/diegobarbosa/ghder/branch/master)


## API EndPoint

https://ghder.herokuapp.com/

By Default, this address will return the Swagger documentation page. Is possible to execute (test) the API in this docs page.

The API has the form: https://ghder.herokuapp.com/api/service/{userName}/{repoName}

The API accepts Json and XML. Calling the API with curl: 

```
curl -X GET "https://ghder.herokuapp.com/api/Service/diegobarbosa/ghdertest" -H "accept: text/json"
curl -X GET "https://ghder.herokuapp.com/api/Service/diegobarbosa/ghdertest" -H "accept: text/xml"

```

Without accepts Header, the api defaults to Json format. Calling from the brownser:
```
https://ghder.herokuapp.com/api/Service/diegobarbosa/ghdertest
```


## Running the docker image locally
Execute the following commands in your CMD:

```console
docker pull diegobarbosa/ghder

docker run -d -p 8080:80 --name ghder diegobarbosa/ghder
```
Access the url http://localhost:8080.


