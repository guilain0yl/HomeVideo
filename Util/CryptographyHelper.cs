using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：JwtBearer
* 项目描述 ：
* 类 名 称 ：AesAHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：JwtBearer
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/14 14:43:20
* 更新时间 ：2019/10/14 14:43:20
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public static class AesHelper
    {
        /// <summary>
        /// aes encrypt
        /// </summary>
        /// <param name="plainText">plain text</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string AESEncrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("AESEncrypt.plainText");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("AESEncrypt.key");
            }

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// aes decrypt
        /// </summary>
        /// <param name="cipherText">cipher text</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string AESDecrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("AESDecrypt.cipherText");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("AESDecrypt.key");
            }

            string plaintext = null;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return (plaintext = srDecrypt.ReadToEnd());
                        }
                    }
                }
            }
        }
    }

    public static class MD5Helper
    {
        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5HashLower(string str) => MD5Hash(str).ToLower();

        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="str">cipher text</param>
        /// <returns></returns>
        public static string MD5Hash(string str)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5Hash(Stream stream)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(stream);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// verify md5 hash
        /// </summary>
        /// <param name="str">cipher text</param>
        /// <param name="hash">hash value for compare</param>
        /// <returns></returns>
        public static bool VerifyMD5Hash(string str, string hash)
        {
            string hashOfInput = MD5Hash(str);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return (0 == comparer.Compare(hashOfInput, hash)) ? true : false;
        }
    }

    public class RsaHelper
    {
        public RsaHelper()
        {
            PrvRsaKey = string.Empty;
            PubRsaKey = string.Empty;
            hashAlgorithmName = HashAlgorithmName.SHA256;
        }

        public RsaHelper(string prvKey) : this(prvKey, string.Empty, HashAlgorithmName.SHA256) { }

        public RsaHelper(string prvKey, string pubKey, HashAlgorithmName hashAlgorithm)
        {
            if (!string.IsNullOrEmpty(prvKey))
            {
                PrvRsaKey = prvKey;
                _privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(prvKey);
            }

            if (!string.IsNullOrEmpty(pubKey))
            {
                PubRsaKey = pubKey;
                _publicKeyRsaProvider = CreateRsaProviderFromPublicKey(pubKey);
            }

            hashAlgorithmName = hashAlgorithm;
        }

        /// <summary>
        /// 私钥签名
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public string Sign(string data)
        {
            if (string.IsNullOrEmpty(PrvRsaKey))
            {
                throw new ArgumentNullException("私钥为空");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            var signatureBytes = privateKeyRsaProvider.SignData(dataBytes, hashAlgorithmName, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);

        }

        /// <summary>
        /// 公钥验签
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public bool Verify(string data, string sign)
        {
            if (string.IsNullOrEmpty(PubRsaKey))
            {
                throw new ArgumentNullException("公钥为空");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signBytes = Convert.FromBase64String(sign);

            var verify = publicKeyRsaProvider.VerifyData(dataBytes, signBytes, hashAlgorithmName, RSASignaturePadding.Pkcs1);

            return verify;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(PrvRsaKey))
            {
                throw new ArgumentNullException("私钥为空");
            }

            if (privateKeyRsaProvider == null)
            {
                throw new Exception("_privateKeyRsaProvider is null");
            }
            return Encoding.UTF8.GetString(privateKeyRsaProvider.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">原文</param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(PubRsaKey))
            {
                throw new ArgumentNullException("公钥为空");
            }

            if (publicKeyRsaProvider == null)
            {
                throw new Exception("publicKeyRsaProvider is null");
            }
            return Convert.ToBase64String(publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
        }

        /// <summary>
        /// 重新加载私钥
        /// </summary>
        /// <param name="prvKey">私钥</param>
        /// <returns></returns>
        public string ReloadPrvKey(string prvKey)
        {
            if (string.IsNullOrEmpty(prvKey))
            {
                throw new ArgumentNullException("私钥为空");
            }

            PrvRsaKey = prvKey;
            _privateKeyRsaProvider = null;

            return PrvRsaKey;
        }

        /// <summary>
        /// 重新加载公钥
        /// </summary>
        /// <param name="pubKey">公钥</param>
        /// <returns></returns>
        public string ReloadPubKey(string pubKey)
        {
            if (string.IsNullOrEmpty(pubKey))
            {
                throw new ArgumentNullException("公钥为空");
            }

            PubRsaKey = pubKey;
            _publicKeyRsaProvider = null;

            return PubRsaKey;
        }

        public string PrvRsaKey { get; private set; }

        public string PubRsaKey { get; private set; }

        public HashAlgorithmName hashAlgorithmName { get; private set; }

        private RSA CreateRsaProviderFromPrivateKey(string privateKey)
        {
            var privateKeyBits = Convert.FromBase64String(privateKey);

            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(rsaParameters);
            return rsa;
        }

        private RSA CreateRsaProviderFromPublicKey(string publicKey)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            var x509Key = Convert.FromBase64String(publicKey);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsa = RSA.Create();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }
        }

        private RSACryptoServiceProvider DecodeRSAPrivateKey(string privateKey)
        {
            byte[] privkey = Convert.FromBase64String(privateKey);
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 2048;

                //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                //rsa.ImportParameters(RSAparams);
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        private RSA privateKeyRsaProvider => _privateKeyRsaProvider ?? CreateRsaProviderFromPrivateKey(PrvRsaKey);

        private RSA _privateKeyRsaProvider = null;

        private RSA publicKeyRsaProvider => _publicKeyRsaProvider ?? CreateRsaProviderFromPrivateKey(PubRsaKey);

        private RSA _publicKeyRsaProvider = null;
    }

    public static class HMACHelper
    {
        public static string HmacSHA256(string secret, string signKey)
        {
            string signRet = string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(signKey)))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                signRet = Convert.ToBase64String(hash);
                signRet = ToHexString(hash); ;
            }
            return signRet;
        }

        //byte[]转16进制格式string
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte b in bytes)
                {
                    strB.AppendFormat("{0:x2}", b);
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }

    public static class SHA256Helper
    {
        public static string SHA_256(string str)
        {
            string signRet = string.Empty;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
                signRet = Convert.ToBase64String(hash);
                signRet = ToHexString(hash); ;
            }

            return signRet;
        }

        public static string SHA_256(byte[] data)
        {
            string signRet = string.Empty;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(data);
                signRet = Convert.ToBase64String(hash);
                signRet = ToHexString(hash); ;
            }

            return signRet;
        }

        //byte[]转16进制格式string
        private static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte b in bytes)
                {
                    strB.AppendFormat("{0:x2}", b);
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }

    public static class SHA1Helper
    {
        public static string SHA_1(string str)
        {
            string signRet = string.Empty;

            using (SHA1 sha = SHA1.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
                signRet = Convert.ToBase64String(hash);
                signRet = ToHexString(hash); ;
            }

            return signRet;
        }

        //byte[]转16进制格式string
        private static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                foreach (byte b in bytes)
                {
                    strB.AppendFormat("{0:x2}", b);
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}
