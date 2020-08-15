docker tag ghder diegobarbosa/ghder:latest

docker login

docker push diegobarbosa/ghder



heroku login

heroku container:login

docker tag ghder registry.heroku.com/ghder/web

docker push registry.heroku.com/ghder/web

heroku container:release web -a ghder

explorer "https://ghder.herokuapp.com/"