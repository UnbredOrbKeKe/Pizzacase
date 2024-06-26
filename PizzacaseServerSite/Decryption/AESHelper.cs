﻿using System.Security.Cryptography;
using System.Text;

namespace PizzacaseServerSite.Decryption
{
    public class AESHelper
    {
        private static string HexedIV = "DB41178B2470BAAE98B6B2B1D885FA86";
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // Vervang dit met een veilige sleutel!
        private static readonly byte[] IV = ConvertHexToBytes(HexedIV); // Vervang dit met een veilige Initialisatievector!

        public static string DecryptStringFromBytes(byte[] cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static byte[] ConvertHexToBytes(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string length must be even.");
            }

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
