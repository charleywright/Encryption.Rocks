// Do not ask how this works, I have no idea and am VERY open to a rewrite, this is very jank

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;

public static partial class Helpers
{
  public static RSAKeyPair GenKeyPair()
  {
    RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
    rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
    AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
    RsaKeyParameters privatekey = (RsaKeyParameters)keyPair.Private;
    RsaKeyParameters publickey = (RsaKeyParameters)keyPair.Public;
    TextWriter textWriter1 = new StringWriter();
    PemWriter pemWriter1 = new PemWriter(textWriter1);
    pemWriter1.WriteObject(publickey);
    pemWriter1.Writer.Flush();
    string print_publickey = textWriter1.ToString();
    TextWriter textWriter2 = new StringWriter();
    PemWriter pemWriter2 = new PemWriter(textWriter2);
    pemWriter2.WriteObject(privatekey);
    pemWriter2.Writer.Flush();
    string print_privatekey = textWriter2.ToString();
    return new RSAKeyPair(print_publickey, print_privatekey);
  }
  public static string RSAEncrypt(string plainText, string publicKey)
  {
    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

    TextReader textReader = new StringReader(publicKey);
    PemReader pr = new PemReader(textReader);
    RsaKeyParameters keys = (RsaKeyParameters)pr.ReadObject();

    OaepEncoding eng = new OaepEncoding(new RsaEngine());
    eng.Init(true, keys);

    int length = plainTextBytes.Length;
    int blockSize = eng.GetInputBlockSize();
    List<byte> cipherTextBytes = new List<byte>();
    for (int chunkPosition = 0;
        chunkPosition < length;
        chunkPosition += blockSize)
    {
      int chunkSize = Math.Min(blockSize, length - chunkPosition);
      cipherTextBytes.AddRange(eng.ProcessBlock(
          plainTextBytes, chunkPosition, chunkSize
      ));
    }
    return Convert.ToBase64String(cipherTextBytes.ToArray());
  }

  public static string RSADecrypt(string cipherText, string privateKey)
  {
    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

    TextReader textReader = new StringReader(privateKey);
    PemReader pr = new PemReader(textReader);
    AsymmetricCipherKeyPair keys = (AsymmetricCipherKeyPair)pr.ReadObject();
    // RsaKeyParameters keys = (RsaKeyParameters)pr.ReadObject();

    OaepEncoding eng = new OaepEncoding(new RsaEngine());
    // eng.Init(false, keys);
    eng.Init(false, keys.Private);

    int length = cipherTextBytes.Length;
    int blockSize = eng.GetInputBlockSize();
    List<byte> plainTextBytes = new List<byte>();
    for (int chunkPosition = 0;
        chunkPosition < length;
        chunkPosition += blockSize)
    {
      int chunkSize = Math.Min(blockSize, length - chunkPosition);
      plainTextBytes.AddRange(eng.ProcessBlock(
          cipherTextBytes, chunkPosition, chunkSize
      ));
    }
    return Encoding.UTF8.GetString(plainTextBytes.ToArray());
  }
}