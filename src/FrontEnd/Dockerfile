FROM microsoft/aspnetcore-build:2.0 as build
WORKDIR /source
COPY ./src/ConferenceDTO/ConferenceDTO.csproj ./src/ConferenceDTO/ConferenceDTO.csproj
COPY ./src/FrontEnd/FrontEnd.csproj ./src/FrontEnd/FrontEnd.csproj
RUN dotnet restore ./src/FrontEnd/FrontEnd.csproj
COPY . .
RUN dotnet publish -c Release -o /app ./src/FrontEnd/FrontEnd.csproj --no-restore

FROM microsoft/aspnetcore:2.0
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FrontEnd.dll"]
