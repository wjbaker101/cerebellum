FROM node:20-alpine AS frontend

ARG AWS_ACCESS_KEY_ID
ARG AWS_SECRET_ACCESS_KEY
ARG AWS_DEFAULT_REGION

RUN apk update
RUN apk add --no-cache aws-cli

WORKDIR /app

COPY ./frontend/package.json .
COPY ./frontend/yarn.lock .
COPY ./frontend/.npmrc .

RUN export CODEARTIFACT_AUTH_TOKEN="$(aws codeartifact get-authorization-token --domain wjb --domain-owner 144953083930 --region eu-west-2 --query authorizationToken --output text)" \
    && npm config set '//wjb-144953083930.d.codeartifact.eu-west-2.amazonaws.com/npm/npm-libs/:_authToken' "${CODEARTIFACT_AUTH_TOKEN}"

RUN yarn run co:login
RUN yarn install

COPY ./frontend .

RUN yarn run build



FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS backend-build

ARG AWS_ACCESS_KEY_ID
ARG AWS_SECRET_ACCESS_KEY
ARG AWS_DEFAULT_REGION

RUN apk add --no-cache aws-cli

WORKDIR /app

RUN mkdir Cerebellum
RUN mkdir Core
RUN mkdir Data

COPY ./backend/Cerebellum/*.csproj ./Cerebellum
COPY ./backend/Core/*.csproj ./Core
COPY ./backend/Data/*.csproj ./Data

RUN aws codeartifact login --tool dotnet --repository DotNetLibs --domain wjb --domain-owner 144953083930

RUN dotnet restore ./Cerebellum
RUN dotnet restore ./Core
RUN dotnet restore ./Data

COPY ./backend .

COPY --from=frontend /app/Cerebellum/wwwroot/ ./Cerebellum/wwwroot/

RUN dotnet publish Cerebellum -c Release -o out



FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS backend_server

WORKDIR /app

COPY --from=backend-build /app/out .

ENTRYPOINT ["dotnet", "Cerebellum.dll"]

EXPOSE 8080