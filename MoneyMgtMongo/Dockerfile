# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY . /source
WORKDIR /source

ARG TARGETARCH

# Build the application with caching of NuGet packages
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

################################################################################

# Create the final stage for running the application.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

ENV MONGO_CONNECTION_STRING = "mongodb+srv://anasmalikp:hOmuncaYls7vJVF3@cluster0.of4g1.mongodb.net/MoneyDB\?retryWrites=true\&w=majority\&appName=Cluster0\&tls=true"
ENV JWT_SECRET = "thisisajwtsecuritykeyforauthenticationandauthorization"

# Install OpenSSL and CA Certificates
RUN apk add --no-cache openssl \
    && apk add --no-cache ca-certificates \
    && update-ca-certificates \
    && openssl version

# Copy the built app from the build stage
COPY --from=build /app .

# Switch to a non-privileged user
USER $APP_UID

# Start the app
ENTRYPOINT ["dotnet", "MoneyMgtMongo.dll"]
