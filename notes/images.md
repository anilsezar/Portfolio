### Local development Test

docker build -t anilsezer/portfolio -f ./Portfolio.WebUi/Dockerfile --platform linux/amd64 .

docker run -d -p 8080:8080 -p 443:443 --name portfolio-webui-container anilsezer/portfolio

### Multi platform build Test

docker buildx build --pull -t anilsezer/portfolio -f ./Portfolio.WebUi/Dockerfile --platform linux/arm64,linux/arm,linux/amd64 . --push

# Random Useful Commands
docker manifest inspect mcr.microsoft.com/dotnet/aspnet | grep architecture



## Build for x64 or arm64:
#### Windows:
cd .\deployment\crons\backup-db\
docker build -t anilsezer/pg_dump .
docker tag IMAGE_ID anilsezer/pg_dump:x64
docker push anilsezer/pg_dump:x64

#### One liner for Rpi:
cd .\deployment\crons\backup-db\
docker build -t anilsezer/pg_dump .
docker tag IMAGE_ID anilsezer/pg_dump:arm64
docker push anilsezer/pg_dump:arm64

**One liner to build & deploy at the rpi:** <br>
todo: change this command to build from the root folder
website:
git pull && docker build -f ./deployment/Dockerfile -t anilsezer/portfolio . && docker push anilsezer/portfolio:latest && sleep 3 && k rollout restart deployment/portfolio-deployment

api: <br>
git pull && docker build -f ./Portfolio.Web.Api/Dockerfile . -t anilsezer/portfolio-api && docker push anilsezer/portfolio-api:latest && sleep 3 && k rollout restart deployment/portfolio-api-deployment

ip-lookup cron:
git pull && docker build -t anilsezer/iplookup-cron-go -f ./CronJobs/IpLookupCron/Dockerfile ./CronJobs/IpLookupCron/ && docker push anilsezer/iplookup-cron-go:latest && sleep 3 && k apply -f deployment/crons/ip-lookup-go-cronjob

### From Root:
docker build -f ./deployment/Dockerfile -t anilsezer/portfolio Portfolio.WebUi/.
docker run -p 8080:80 anilsezer/portfolio:latest