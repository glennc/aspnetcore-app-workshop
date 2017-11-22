FROM microsoft/aspnetcore-build:2.0 as build
ARG csproj=./src/BackEnd/BackEnd.csproj
WORKDIR /source
COPY ./src/ConferenceDTO/ConferenceDTO.csproj ./src/ConferenceDTO/ConferenceDTO.csproj
COPY $csproj $csproj
RUN dotnet restore $csproj
COPY . .
RUN dotnet publish -c Release -o /app ./src/BackEnd/BackEnd.csproj --no-restore

FROM microsoft/aspnetcore:2.0
WORKDIR /app
EXPOSE 56009
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BackEnd.dll"]
