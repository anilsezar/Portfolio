kubectl run -it --rm --restart=Never --image=busybox debug -- wget -qO- jaeger-otlp-service:4317


## Ansible
ansible -i inventory.yml all -m ping -u $(whoami)
ansible -i inventory.yml all -a "free -h" -u $(whoami)
ansible -i inventory.yml all -a "date" -u $(whoami)
ansible -i inventory.yml all -m setup -u $(whoami)
ansible -i inventory.yml all -m apt -u $(whoami) -a "name=packagename state=present"
ansible -i inventory.yml all -b -a "tail var/log/messages"
ansible -i inventory.yml all -m setup

https://docs.ansible.com/ansible/latest/collections/ansible/builtin/service_module.html:
Starts the service and enables it even after device restarts
ansible -i inventory.yml all -m service -u $(whoami) -a "name=ntpd state=started enabled=yes" 

ansible-playbook -i inventory.yml update_ubuntu.yml

ansible-playbook -i inventory.yml main.yml -u $(whoami) --diff
ansible-playbook -i inventory.yml main.yml -u $(whoami) --check

## Graceful shutdown
k drain raspberrypi4 --ignore-daemonsets --delete-emptydir-data
k cordon NODENAME

k uncordon NODENAME

## Node Maintenance

k drain raspberrypi4 --ignore-daemonsets --delete-emptydir-data

sudo apt update
sudo apt upgrade

k uncordon NODENAME


## DB
Get superuser:
SELECT usename FROM pg_user WHERE usesuper = TRUE;

Terminate idle connections
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'postgres' AND state = 'idle';

Connections:
SELECT COUNT(*) FROM pg_stat_activity;
SELECT pid, usename, datname, state FROM pg_stat_activity;
SELECT * FROM pg_stat_activity;
SHOW max_connections;

# Adding ConfigMap
k apply -f terminate-idle-db-connections-query-config-map.yaml


# Debug via nikolaka
k apply -f netshoot-debug-pod.yaml
kubectl exec netshoot-debug-pod -- curl -k https://10.152.183.1:443


## Fail2Ban
### Check Fail2Ban's All Jails

for jail in $(sudo fail2ban-client status | grep "Jail list:" | sed -E 's/^[^:]+:[ \t]+//' | tr ',' ' '); do
echo "Jail: $jail"
sudo fail2ban-client status $jail | grep "Banned IP list:"
done

### Check its ban or unban logs:

sudo cat /var/log/fail2ban.log | grep "Ban "

## Migrations
dotnet ef migrations add MIGRATIONNAME --startup-project .\Portfolio.WebUi\ --project .\Portfolio.DataAccess\ --output-dir Migrations
dotnet ef database update --startup-project .\Portfolio.WebUi\

### Check for ports:
kubectl get svc --all-namespaces -o json | jq -r '.items[] | select(.spec.type == "NodePort") | .spec.ports[] | .nodePort'

#### Get Sqlite file:
kubectl cp portfolio-deployment-6c69c56f8-ct4qn:/app/WebApp.db ~/WebApp.db
getDb

**One liner to build & deploy at the rpi:** <br>
todo: change this command to build from the root folder
website:
git pull && docker build -f ./Portfolio.WebUi/Dockerfile -t anilsezer/portfolio . && docker push anilsezer/portfolio:latest && sleep 3 && k rollout restart deployment/portfolio-deployment

api: <br>
git pull && docker build -f ./Portfolio.Web.Api/Dockerfile . -t anilsezer/portfolio-api && docker push anilsezer/portfolio-api:latest && sleep 3 && k rollout restart deployment/portfolio-api-deployment

ip-lookup cron:
git pull && docker build -t anilsezer/iplookup-cron-go -f ./CronJobs/IpLookupCron/Dockerfile ./CronJobs/IpLookupCron/ && docker push anilsezer/iplookup-cron-go:latest && sleep 3 && k apply -f deployment/crons/ip-lookup-go-cronjob

### From Root:
docker build -f ./deployment/Dockerfile -t anilsezer/portfolio Portfolio.WebUi/.
docker run -p 8080:80 anilsezer/portfolio:latest


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

# Get pods:
sudo crictl --runtime-endpoint unix:///run/containerd/containerd.sock ps
journalctl -u containerd --since "60 minutes ago" | grep -E "error|warning"


kubectl logs -n kube-system kube-proxy-nnwx7

kubectl logs -n kube-system coredns-6f6b679f8f-cbfsr
kubectl describe pod coredns-6f6b679f8f-cbfsr -n kube-system

kubectl describe pod kube-proxy-tv4w2 -n kube-system


kubectl logs -n kube-flannel kube-flannel-ds-st4k7 --previous


sudo dmesg | grep -i apparmor
sudo grep 'audit' /var/log/syslog | grep 'DENIED'

systemctl restart containerd


DEBUG:
kubectl taint nodes --all node-role.kubernetes.io/control-plane-
kubectl apply -f https://k8s.io/examples/application/nginx-with-request.yaml
kubectl get pods
kubectl describe pod nginx-deployment-67d4bdd6f5-w6kd7

kubectl describe pod kube-flannel-ds-72krz -n kube-flannel


CHECKLIST:
/etc/modules-load.d/containerd.conf: Should have: overlay & br_netfilter

Sources:
https://www.danielcolomb.com/2023/07/16/building-a-kubernetes-cluster-with-raspberry-pis-and-containerd/

kubectl events

https://linuxconfig.org/how-to-disable-apparmor-on-ubuntu-20-04-focal-fossa-linux

sudo crictl image
systemctl status kubelet

kubectl cluster-info
kubectl cluster-info dump