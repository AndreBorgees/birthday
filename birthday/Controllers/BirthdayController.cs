using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace birthday.Controllers
{
    [ApiController]
    [Route("/aniversario")]
    public class BirthdayController : ControllerBase
    {
       
        private readonly ILogger<BirthdayController> _logger;

        public BirthdayController(ILogger<BirthdayController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retorna quantos dias falta para o aniversário do Plebeu Alef
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBirthday()
        {
            DecryptFile("h99+x9qTGCWpRg23ixbwMszOyh0qdjbOC7Pi6r4KzN/pEBdPurgwnAzu6VeVGBPipbI3lOhI0qBAsC+mGZHhaBVbC6Vy5wJTZb4h8eqDG1PgOnwwID8m1oI+DQvuwutMR1BjSpK1JjA0KCJxDGVdD6N/xkDSmYSV/BFKh/OD/Lt3m7vc4LXG4Pbe3iHMjFz/tg/IhNbiLsmGbSOgJJuwb44SmkXFgdxZxp4W/0n/h3tY6B2lsFiggjUa1IC9dNcvYWQSOQ/Z364pWINnylTTiZmz3lUFw1gE1I2weYmg9V3KpVIx6HYX+JR4nTe1qCgdc4QNQ48ujpVGS+ueRlpBXw==");
            var stringo = EncryptFile("andreborges");

            var response = await CalculateBirthday();

            return Ok(response + " " + stringo);
        }

        private  string MakeKey()
        {
            //lets take a new CSP with a new 2048 bit rsa key pair
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

            //how to get the private key
            RSAParameters privKey = csp.ExportParameters(true);

            //and the public key ...
            RSAParameters pubKey = csp.ExportParameters(false);
            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();

            }

            string privKeyString;
            {
                //we need some buffer
                var sw = new StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privKey);
                //get the string from the stream
                privKeyString = sw.ToString();
            }

            return privKeyString + " " + pubKeyString;
        }

        private string EncryptFile(string texto)
        {
            //converting the public key into a string representation
            string pubKeyString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Exponent>AQAB</Exponent>\r\n  <Modulus>sMKw02VrpvwV8G9UroUa3WYMDDgHCxLGPhCf4cuZgUmZI+S9M+NRS0IpEqpvZfvJqaWiTCjHFCCe4hthMM8UFeilBQFMfuuyuVz0H3PHmlJpOR6bpfbRxhikm7XTX6H2eoVBzqgDzVtrFCjpHPnMsbr4pgqDV42ARujeiZrfwD99tGmbpdx+OaPvJ7Nf8dSGPZuueWk4HZF9Cn83oJh0NaTpNEdukezZG+qdZfQ5gpB4VbHO1Cj4xjW1aXA+YMMl+nGzDn8HeiEH71kSH0zjTg7Mjb2DLNbdGCOxYc2CWro2OAWzgd6NeFmDRBR4reNVt2lEVDinx+WSBwbQtXQuLQ==</Modulus>\r\n</RSAParameters>";

            //get a stream from the string
            var sr = new StringReader(pubKeyString);

            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

            //get the object back from the stream
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            byte[] bytesPlainTextData = Encoding.ASCII.GetBytes(texto); 

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);
            //we might want a string representation of our cypher text... base64 will do
            string encryptedText = Convert.ToBase64String(bytesCipherText);

            return encryptedText;
          
        }

        private void DecryptFile(string encryptedText)
        {
            //we want to decrypt, therefore we need a csp and load our private key
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

            string privKeyString;
            {
                privKeyString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <D>O2di19io4jisAt7XfcNb8PYfrGeT7mPD3g3mPZMYJrweTFLOR0bpBjrY5N4EjCifcHUq4x99MEV6QBDli+q8HFTQSKtUEv+RU/xqSru1ZCa4GCsyMgeOHl4fy/jCbcxXsntgT1xl0rkbdm39UfCdm0lz0RGNbHowlyQhzesPsWLVuli8W9cd4RDqm/h9CkADPXLWM294NS2joedchklCbjoGrYIZTiAPIbP/HhH38oS1vehUxzPgVjf12nRDJqBDlXGgho/aifOy3ihytmFWyLIBpZpjaaqWq/U+0gZEjfwKCjel6JkI+3pxCevs8qlHqOdoPUmfkN1tjvVX8vI2fQ==</D>\r\n  <DP>DAVDT6EetjIuoWSG4obk5Z+60ul0AQnOyanaJ07kQjURmYI56m/2Z/VVcwPimsoz7dSrw6nYlqJRoNaa4hoDJEQZO7/O8WvsAC8j1/KZ4NNxDgdo541Y5U27OjW+MIH6DnE6XCVAfVNKatfXv+02YHzFdDu9taOxXOz21PE/JtU=</DP>\r\n  <DQ>D+Q9VkR5PdPtHvaL22AG98ZzjtGMoeCtbv0U2E/XdBbXhkdt3+N+PRTdgAZEnWQpYK37DqtD5REGS04R/debdK5eTTiOFUvy2qGT4QVr8xVmy1k9K3W2z0YW/Hgtioe73Bmlgu+ii3uTFF603M4wG7/3Isl62AcsKnZ48PM3HwU=</DQ>\r\n  <Exponent>AQAB</Exponent>\r\n  <InverseQ>vZAKQnt9QrBDvWVer0PS20qW073UrFXuYFugD2WYwnJi/nsrs46xtBMirDha8baRklRVc1u0Y7zVFfjVkjIRsTN9k8wzkVslttIyMz7Vujx1tjvBAR0tD8Qh2sNiwPq3M4Ge/WBOMyFFnRX6dFxPCv/3iIfZD/z05fJQjumFCV0=</InverseQ>\r\n  <Modulus>sMKw02VrpvwV8G9UroUa3WYMDDgHCxLGPhCf4cuZgUmZI+S9M+NRS0IpEqpvZfvJqaWiTCjHFCCe4hthMM8UFeilBQFMfuuyuVz0H3PHmlJpOR6bpfbRxhikm7XTX6H2eoVBzqgDzVtrFCjpHPnMsbr4pgqDV42ARujeiZrfwD99tGmbpdx+OaPvJ7Nf8dSGPZuueWk4HZF9Cn83oJh0NaTpNEdukezZG+qdZfQ5gpB4VbHO1Cj4xjW1aXA+YMMl+nGzDn8HeiEH71kSH0zjTg7Mjb2DLNbdGCOxYc2CWro2OAWzgd6NeFmDRBR4reNVt2lEVDinx+WSBwbQtXQuLQ==</Modulus>\r\n  <P>xgl7OxrLDpn84jbY8MxZ3xdTL8S5f+xs2lRrrKofu5zh62VBjhXvQ0woLefoO4PX2GWGQ4vr89TFYjgZRbBH9zLvpPEdS7xkJD2GLwHKa69K37lmQYdNAltqse/O+nnsNLlkGCwPGJGuR3MaXsp6H91fsdKP+/i6aKkbR7ICUmM=</P>\r\n  <Q>5H8AelETHO80K8o9cKkTbAdgVo5nT7cr5vq2DYSmBRJJwFzLDeS2Ab834MJd44MFGiMIPJBgLE9E4HSr3CTqySxQLjc6LbhXiaDimmoYESZo9VSdE5hVpqLH3c4DhaHG7VNTt/LUB4hmcRqxAS6rEzZq3jknvLcb1lyGw3aDGi8=</Q>\r\n</RSAParameters>\r\n";
                //get a stream from the string
                var sr = new StringReader(privKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                RSAParameters privKey = (RSAParameters)xs.Deserialize(sr);
                csp.ImportParameters(privKey);
            }

           
            byte[] bytesCipherText = Convert.FromBase64String(encryptedText);

            //decrypt and strip pkcs#1.5 padding
            byte[] bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            var str = System.Text.Encoding.Default.GetString(bytesPlainTextData);

            //get our original plainText back...

        }


        private async Task<string> CalculateBirthday()
        {
            var monthsToAlefBirthday = 0;
            var daysToAlefBirthday = 0;

            var alefBirthday = Convert.ToDateTime("10/11/2023");
            var today = DateTime.Now;

            if (today.Day > 10)
            {
                monthsToAlefBirthday = alefBirthday.Month - today.Month - 1;
                var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
                var restDays = daysInMonth - today.Day;
                daysToAlefBirthday = alefBirthday.Day + restDays;
            }
            else
            {
                monthsToAlefBirthday = alefBirthday.Month - today.Month;
                daysToAlefBirthday = alefBirthday.Day - today.Day;
            }

            var message = MessageMount(monthsToAlefBirthday, daysToAlefBirthday);

            return message;
        }

        private string MessageMount(int monthsToAlefBirthday, int daysToAlefBirthday)
        {
            var message = "";
            switch (monthsToAlefBirthday)
            {
                case 1:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {monthsToAlefBirthday} MÊS E {daysToAlefBirthday} DIA PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                            return message;
                    }

                    message = $"FALTAM {monthsToAlefBirthday} MÊS E {daysToAlefBirthday} DIAS PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                    break;
                case > 1:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {monthsToAlefBirthday} MESES E {daysToAlefBirthday} DIA PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                            return message;
                    }
                    message = $"FALTAM {monthsToAlefBirthday} MESES E {daysToAlefBirthday} DIAS PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                    break;
                default:
                    switch (daysToAlefBirthday)
                    {
                        case 1:
                            message = $"FALTAM {daysToAlefBirthday} DIA PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                            return message;
                    }
                    message = $"FALTAM {daysToAlefBirthday} DIAS PARA O ANIVERSÁRIO DO PLEBEU ALEF";
                    break;
            }
            return message;
        }
    }
}
