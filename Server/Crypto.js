const crypto = require("crypto");

module.exports.generateKeyPair = function () {
  const { privateKey, publicKey } = crypto.generateKeyPairSync("rsa", {
    modulusLength: 2048,
    publicKeyEncoding: {
      type: "spki",
      format: "pem",
    },
    privateKeyEncoding: {
      type: "pkcs8",
      format: "pem",
    },
  });
  return { private: privateKey, public: publicKey };
};

module.exports.encryptWithPublicKey = function (text, publicKey) {
  const buffer = Buffer.from(text);
  const encrypted = crypto.publicEncrypt(publicKey, buffer);
  return encrypted.toString("base64");
};

module.exports.decryptWithPrivateKey = function (text, privateKey) {
  const buffer = Buffer.from(text, "base64");
  const decrypted = crypto.privateDecrypt(privateKey, buffer);
  return decrypted.toString("utf8");
};
