#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
#WORKDIR /app
#
## Copy everything
#COPY . ./
## Restore as distinct layers
#RUN dotnet restore
## Build and publish a release
#RUN dotnet publish -c Release -o out
#
## Build runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:6.0.2
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "Agems.dll"]
#EXPOSE 3000
#
##################

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Agems.csproj", "."]
#RUN dotnet restore "./Agems.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "Agems.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Agems.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Agems.dll"]



#####################################3

FROM centos:7 AS base

# Add Microsoft package repository and install ASP.NET Core
RUN rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm \
    && yum install -y aspnetcore-runtime-6.0

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:30001

WORKDIR /app

# ... remainder of dockerfile as before
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Agems.csproj", "."]
RUN dotnet restore "./Agems.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Agems.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Agems.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agems.dll"]