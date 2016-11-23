using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Model
{
    public class Crypt
    {
        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="key">Private key</param>
        /// <param name="value">Value to be encrypted</param>
        /// <returns>Base64 encoded string</returns>
        public static string Encrypt(string key, string value)
        {
            if (value == string.Empty)
                return string.Empty;
            // Private key has to be exactly 16 characters
            if (key.Length > 16)
            {
                // Cut of the end if it exceeds 16 characters
                key = key.Substring(0, 16);
            }
            else
            {
                // Append zero to make it 16 characters if the provided key is less
                while (key.Length < 16)
                {
                    key += "0";
                }
            }

            // We'll be using AES, CBC mode with PKCS#7 padding to encrypt
            SymmetricKeyAlgorithmProvider aesCbcPkcs7 = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            // Convert the private key to binary
            IBuffer keymaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

            // Create the private key
            CryptographicKey k = aesCbcPkcs7.CreateSymmetricKey(keymaterial);

            // Creata a 16 byte initialization vector
            IBuffer iv = keymaterial;

            // Encrypt the data
            byte[] plainText = Encoding.UTF8.GetBytes(value); // Data to encrypt

            IBuffer buff = CryptographicEngine.Encrypt(k, CryptographicBuffer.CreateFromByteArray(plainText), iv);


            return CryptographicBuffer.EncodeToBase64String(buff);
        }

        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="key">Private key</param>
        /// <param name="value">Encrypted string in Base64 format</param>
        /// <returns>Original value</returns>
        public static string Decrypt(string key, string value)
        {
            if (value == string.Empty)
                return string.Empty;

            if (key.Length > 16)
            {
                key = key.Substring(0, 16);
            }
            else
            {
                // Fill key
                while (key.Length < 16)
                {
                    key += "0";
                }
            }


            IBuffer val = CryptographicBuffer.DecodeFromBase64String(value);

            // Use AES, CBC mode with PKCS#7 padding (good default choice)
            SymmetricKeyAlgorithmProvider aesCbcPkcs7 =
                SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            IBuffer keymaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

            // Create an AES 128-bit (16 byte) key
            CryptographicKey k = aesCbcPkcs7.CreateSymmetricKey(keymaterial);

            // Creata a 16 byte initialization vector
            IBuffer iv = keymaterial;// CryptographicBuffer.GenerateRandom(aesCbcPkcs7.BlockLength);

            //IBuffer val = CryptographicBuffer.DecodeFromBase64String(value);

            IBuffer buff = CryptographicEngine.Decrypt(k, val, iv);

            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buff);

        }
    }
}
