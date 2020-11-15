# Encryption.Rocks

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/charleywright/Encryption.Rocks/blob/master/LICENSE.ttx)

Encryption Rocks is an open-source chat service that uses RSA encryption to encrypt messages, then sends them over websockets, using [socket.io](https://socket.io). The server is written in [Node.js](https://nodejs.org/) and the client is written in [.NET Core](https://dotnet.microsoft.com), making it completely cross-platform.

### Dependencies & thanks

- [socket.io-client-csharp](https://github.com/doghappy/socket.io-client-csharp/) - An amazing implementation of socket.io for C#
- [socket.io](https://socket.io/) - A really nice implementation of websockets with good documentation
- [Docker](https://www.docker.com/) - A really useful tool for bundling servers
- [.NET Core](https://github.com/dotnet/core) - For allowing us to write cross-platform apps using C#

### Compiling / Usage

To compile the client you will require the [.NET CLI](https://dotnet.microsoft.com/download/dotnet-core)
To run the server you will need either [Node.js](https://nodejs.org/) or [Docker](https://www.docker.com/get-started)

Compile the client:

- For Windows:

  ```sh
  dotnet publish -r win-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None
  ```

- For Mac:

  ```sh
  dotnet publish -r osx-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None
  ```

- For Linux:
  ```sh
  dotnet publish -r linux-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None
  ```

You will also need to run the server, although we recommend using [Docker](#server-docker):

```sh
cd Server
npm start
```

### Development

Encryption.rocks uses the .NET CLI and Nodejs, so both the server and client will have hot-reload enabled, so when you edit your code, you can see your changes immediately. To get started, run the server in one tab or window:

```sh
cd Server
npm install
npm install nodemon
nodemon index.js
```

Then in another window/tab run the client:

```sh
cd App
dotnet watch run
```

### <a id="server-docker"></a> Docker

We recommend deploying the server using Docker as it is simple and easy

By default, the Dockerfile will expose port 3000, if you want to change this make sure to update the second line of `index.js`

```sh
cd Server
docker build -t <youruser>/encryption-rocks-server .
```

Once done, run the Docker image and map the port to whatever you wish on your host. In this example, we simply map port 3000 of the host to port 3000 of the Docker image:

```sh
docker run -d -p 3000:3000 <youruser>/encryption-rocks-server
```

### Todo

- Add room support, so one server can handle multiple rooms
- Password protection for rooms / servers

## License

MIT
