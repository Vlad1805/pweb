# MobyLabWebProgramming
docker compose -f .\docker-compose.yml -p mobylab-web-app up -d
dotnet tool install --global dotnet-ef --version 6.*
dotnet ef migrations add <migration_name> --context WebAppDatabaseContext --project .\MobyLabWebProgramming.Infrastructure --startup-project .\MobyLabWebProgramming.Backend