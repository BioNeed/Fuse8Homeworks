FROM mcr.microsoft.com/dotnet/sdk:7.0 AS prepare-restore
LABEL description="������ ��� �������������� � CurrencyApi API"
EXPOSE 5236
EXPOSE 7228
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2
WORKDIR /src
COPY . .
RUN dotnet subset restore InternalAPI/InternalAPI.csproj --root-directory /src --output restore_subset/ 

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY --from=prepare-restore /src/restore_subset .
RUN dotnet restore InternalAPI/InternalAPI.csproj

COPY . .
RUN dotnet publish InternalAPI/InternalAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "InternalAPI.dll"]