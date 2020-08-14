docker tag trustlyghder diegobarbosa/trustlyghder:latest

docker login

docker push diegobarbosa/trustlyghder



heroku login

heroku container:login

docker tag trustlyghder registry.heroku.com/trustlyghder/web

docker push registry.heroku.com/trustlyghder/web

heroku container:release web -a trustlyghder

explorer "https://trustlyghder.herokuapp.com/"