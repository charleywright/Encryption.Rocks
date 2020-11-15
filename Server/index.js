const Crypto = require("./Crypto");
const port = process.env.PORT || 3000;
const io = require("socket.io")(port);
const uuid = require("uuid").v4;
console.clear();
console.log("Server running on port " + port);

const { private: privKey, public: pubKey } = Crypto.generateKeyPair();

let clients = {};
io.on("connection", (socket) => {
  socket.emit("auth-needed");
  socket.on("client-auth", (data) => {
    const parsed = JSON.parse(data);
    const id = uuid();
    clients[id] = {
      name: parsed.ClientName,
      key: parsed.ClientKey,
      socket: socket,
    };
    socket.broadcast.emit("user-join", parsed.ClientName);
    socket.emit(
      "auth-succesful",
      JSON.stringify({ ServerKey: pubKey, ClientId: id })
    );
  });

  socket.on("message-send", (data) => {
    let decoded = JSON.parse(data);
    let user = clients[decoded.sender];
    decoded.content = Crypto.decryptWithPrivateKey(decoded.content, privKey);
    let res = [];
    for (const client in clients) {
      res.push({
        content: Crypto.encryptWithPublicKey(
          decoded.content,
          clients[client].key
        ),
        sender: `${decoded.sender} - ${user.name}`,
        aim: client,
      });
    }
    io.emit("new-message", JSON.stringify(res));
  });

  socket.on("manual-disconnect", (data) => {
    io.emit("user-leave", data);
  });
});
