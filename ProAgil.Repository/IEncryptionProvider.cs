namespace ProAgil.Repository
{
    public interface IEncryptionProvider
    {
         string Encrypt(string dataToEncrypt);
         string Decrypt(string dataToDecrypt);
    }
}