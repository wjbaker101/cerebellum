FROM node:20-alpine AS frontend

WORKDIR /app

COPY ./frontend/package.json .
COPY ./frontend/yarn.lock .

RUN yarn install

COPY ./frontend .

RUN yarn run build



FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS backend-build

WORKDIR /app

RUN mkdir Cerebellum
RUN mkdir Core
RUN mkdir Data

COPY ./backend/Cerebellum/*.csproj ./Cerebellum
COPY ./backend/Core/*.csproj ./Core
COPY ./backend/Data/*.csproj ./Data

RUN dotnet restore ./Cerebellum
RUN dotnet restore ./Core
RUN dotnet restore ./Data

COPY ./backend .

COPY --from=frontend /app/dist ./Cerebellum/wwwroot

RUN dotnet publish Cerebellum -c Release -o out



FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS backend_server

WORKDIR /app

COPY --from=backend-build /app/out .

ENTRYPOINT ["dotnet", "Cerebellum.dll"]

EXPOSE 8080