using System;
using System.Text;

namespace RC4Cipher
{
    class Program
    {
        static void Main(string[] args)
        {
            //Preseted key
            byte[] key = Encoding.ASCII.GetBytes("llavesecreta");

            //Key-scheduling algorithm
            byte[] state = KSA(key);

            //Pseudo-random generation algorithm
            byte[] keyStream = PRGA(state);

            //Encription
            //c[pos] = p[pos] xor k[pos]
            byte[] p = Encoding.ASCII.GetBytes("secretoPro");

            byte[] c = new byte[p.Length];

            Console.WriteLine("Plain text: " + Encoding.ASCII.GetString(p));
            Console.WriteLine("Plain text (Hex): " + BitConverter.ToString(p));
            Console.WriteLine("Key: " + Encoding.ASCII.GetString(key));

            for (int i = 0; i < p.Length; i++)
            {
                //c[pos] = p[pos] xor k[pos] with explicit conversion to bytes
                c[i] = (byte)(p[i] ^ keyStream[i]);
            }

            //Print result
            //P[pos] = c[pos] xor k[pos]
            Console.WriteLine("Ciph(ASCII): " + Encoding.ASCII.GetString(c));
            Console.WriteLine("Ciph(Hex): " + BitConverter.ToString(c));

            //Decription
            byte[] P = new byte[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                //P[pos] = c[pos] xor k[pos] with explicit conversion to bytes
                P[i] = (byte)(c[i] ^ keyStream[i]);
            }

            //Print result
            Console.WriteLine("Decripted(ASCII): " + Encoding.ASCII.GetString(P));

            Console.WriteLine();
            Console.WriteLine("Key stream:");
            for (int x = 0; x < 16; x++)
            {
                Console.Write(BitConverter.ToString(keyStream, x * 16, 16).Replace("-", " ") + " ");
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static byte[] KSA(byte[] key)
        {
            byte[] s = new byte[256];

            int j = 0;

            for (int i = 0; i < 256; i++)
            {
                s[i] = (byte)i;
            }
            
            //Permuta el arreglo respecto a la llave pre-establecida
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) % 256;

                byte bS = s[i];
                s[i] = s[j];
                s[j] = bS;
            }

            return s;

        }

        static byte[] PRGA(byte[] state)
        {
            byte[] kStream = new byte[state.Length];

            int i = 0;
            int j = 0;

            //Permuta para generar una mezcla pseudo-random en el arreglo
            for(int ind = 0; ind < state.Length; ind++)
            {
                //Valor pseudo-random respecto al primer arreglo creado
                i = (i + 1) % 256;
                j = (j + state[i]) % 256;

                //intercambio de posiciones entre 2 valores en el arreglo de forma pseudo-random
                byte bS = state[i];
                state[i] = state[j];
                state[j] = bS;

                //Se genera el Key Stream
                kStream[ind] = state[(state[i] + state[j]) % 256];
            }

            return kStream;
        }
    }
}
