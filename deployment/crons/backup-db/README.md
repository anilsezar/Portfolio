docker buildx build -t anilsezer/pg_dump:latest --platform linux/amd64,linux/arm64 . && docker push anilsezer/pg_dump:latest