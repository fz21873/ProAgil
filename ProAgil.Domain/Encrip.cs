using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Input;
namespace ProAgil.Domain
{
    public static class Encrip
    {

      public static string Base64Encode(string word) 
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(word);
            return Convert.ToBase64String(byt);
        }
        public static string Base64Decode(string word)
        {
            byte[] b = Convert.FromBase64String(word);
            return System.Text.Encoding.UTF8.GetString(b);
        }


       /* public byte[] Key { get; set; }
        public byte[] IniVetor { get; set; }
        public Aes Algorithm { get; set; }
 
        public  Encrip()
        {

           byte[] key=new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            this.Key =key;
            this.IniVetor = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            this.Algorithm = Aes.Create();
        }
 
        public Encrip(byte[] key, byte[] iniVetor)
        {
             key =  
            this.Key = key;
            this.IniVetor = iniVetor;
            this.Algorithm = Aes.Create();
        }
 
        public string Encrypt(string entryText)
        {
            byte[] symEncryptedData;
 
            var dataToProtectAsArray = Encoding.UTF8.GetBytes(entryText);
            using (var encryptor = this.Algorithm.CreateEncryptor(this.Key, this.IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToProtectAsArray, 0, dataToProtectAsArray.Length);
                cryptoStream.FlushFinalBlock();
                symEncryptedData = memoryStream.ToArray();
            }
            this.Algorithm.Dispose();
            return Convert.ToBase64String(symEncryptedData);
        }
 
        public string Decrypt(string entryText)
        {
            var symEncryptedData = Convert.FromBase64String(entryText);
            byte[] symUnencryptedData;
            using (var decryptor = this.Algorithm.CreateDecryptor(this.Key, this.IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(symEncryptedData, 0, symEncryptedData.Length);
                cryptoStream.FlushFinalBlock();
                symUnencryptedData = memoryStream.ToArray();
            }
            this.Algorithm.Dispose();
            return System.Text.Encoding.Default.GetString(symUnencryptedData);
        }/*
 /*static string keyString="E546C8DF278CD5931069B522E695D4D4";
 public static string Encriptar(this string _cadenaAencriptar)
{
 
 var resultadoA="";
string result = string.Empty;

byte[] encryted =System.Text.UTF8Encoding.UTF8.GetBytes(_cadenaAencriptar);
//result = Convert.ToBase64String(encryted);
resultadoA=char.ConvertFromUtf32Convert.ToBase64String(encryted);
result=Replazar(resultadoA.Trim());
return result;
}

/// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
public static string DesEncriptar(this string _cadenaAdesencriptar)
{

string result  = string.Empty;
byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
//result = System.Text.UTF8Encoding.UTF8.GetString(decryted, 0, decryted.Length);
result = System.Text.UTF8Encoding.UTF8.GetString(decryted);

return result;
}

public static string EncryptString(string text)
        {
            var key = Encoding.UTF8.GetBytes(keyString);
            var resultadoA="";
           // var resultadoB="";
            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        resultadoA=Convert.ToBase64String(result);
                       // resultadoA=Replazar(resultadoA);
                        return resultadoA;
                    }
                }
            }
        }



  private static string Replazar(string cadena){
       //string resul="";
       string caract="";
       string novaCadena="";
       for(int i =0;i<cadena.Length-1;i++){
           caract=cadena.Substring(i);
           if(caract=="/" || caract=="="  || caract=="-"  ||  caract=="&"  ||
             caract=="+"  || caract=="-"  || caract==":"  || caract=="_"   ||
             caract=="$"  || caract=="@"  || caract=="!"  || caract=="#"   ||
             caract=="%"  || caract=="*"  || caract==">"  || caract=="<"   ||
             caract=="?"  || caract=="|"  || caract=="-")
             {
               caract=caract.Replace(caract,"");
               novaCadena=novaCadena+caract;
               caract="";
             }

             else{

                   novaCadena=novaCadena+caract;
                  // resul=novaCadena;
                   caract=""; 

                 }
             
       }
         return novaCadena;
  }
public static string DecryptString(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }*/

    }
}