using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ProAgil.Repository
{
    public static class Encritar
    {
        public static string Encriptar(this string _cadenaAencriptar)
{
string result = string.Empty;
byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
result = Convert.ToBase64String(encryted);
return result;
}

    }
}