const Crypto = require("./Crypto");
const io = require("socket.io")(process.env.PORT || 3000);
const uuid = require("uuid").v4;

const { private: privKey, public: pubKey } = Crypto.generateKeyPair();

let clients = {};
io.on("connection", (socket) => {
  socket.emit("auth-needed");
  socket.on("message", (data) => console.log(data));
  socket.on("client-auth", (data) => {
    const parsed = JSON.parse(data);
    const id = uuid();
    clients[id] = {
      name: parsed.ClientName,
      key: parsed.ClientKey,
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
    // console.log(JSON.stringify(user, null, 2));
    // console.log(decoded.content);
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
    console.log(JSON.stringify(res));
    io.emit("new-message", JSON.stringify(res));
  });
});
