FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY netart.csproj .
RUN dotnet clean
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app --no-cache

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./netart"]
EXPOSE 5000/tcp
EXPOSE 5001/tcp