# We're doing what is called a multi-stage build. This enables
# you to build a docker image and then use only some of that image
# in another one (i.e. the next "FROM" statement you see) or base
# that next one off of an earlier stage build (i.e. a "FROM" that
# appears earlier in this file).
#
# You can also name these using the syntax "FROM <image> AS <name>"
# and then refer to those names later

# Start with ASP.Net 3.1 as the base and add in our base URL/Port
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

# This one is going to build our application so we'll copy over
# the csproj (and do a `dotnet restore`) and then copy all the
# source files and build the application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["TestRest.csproj", "./"]
RUN dotnet restore "TestRest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestRest.csproj" -c Release -o /app/build

# Now we're going to create an actual publishable version of
# the C# app
FROM build AS publish
RUN dotnet publish "TestRest.csproj" -c Release -o /app/publish

# This is the actual image that we'll be deploying into docker
# and kubernetes. It will have ASP.Net 3.1 and only our
# published app's files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestRest.dll"]

# These will be added by kubernetes, but if you wanted to run mysql
# locally and try out the docker image, you can uncomment these
# (and set them to whatever your local MySQL has, except leave the
# url as it is - this lets a container find the localhost outside
# itself)
# ENV MYSQL_ROOT_USERNAME=kubeuser
# ENV MYSQL_ROOT_PASSWORD=kube1234!
# ENV MYSQL_URL=host.docker.internal
