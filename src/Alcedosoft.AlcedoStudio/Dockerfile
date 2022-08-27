FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Alcedosoft.AlcedoStudio/Alcedosoft.AlcedoStudio.csproj", "Alcedosoft.AlcedoStudio/"]
RUN dotnet restore "Alcedosoft.AlcedoStudio/Alcedosoft.AlcedoStudio.csproj"
COPY . .
WORKDIR "/src/Alcedosoft.AlcedoStudio"
RUN dotnet build "Alcedosoft.AlcedoStudio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Alcedosoft.AlcedoStudio.csproj" -c Release -o /app/publish

FROM nginx AS final
EXPOSE 80
EXPOSE 443
COPY alcedo.studio.crt /etc/nginx/alcedo.studio.crt
COPY alcedo.studio.key /etc/nginx/alcedo.studio.key
COPY alcedo.studio.conf /etc/nginx/conf.d/alcedo.studio.conf
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html