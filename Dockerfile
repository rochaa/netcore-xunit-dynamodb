FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# Build da aplicacao
COPY . ./
RUN dotnet publish ./src/gidu.WebAPI -c Release -o out

# Build da imagem
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build /app/src/gidu.WebAPI/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "gidu.WebAPI.dll"]