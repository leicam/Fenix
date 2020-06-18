using System.Text;

namespace Fenix.nsExtensions
{
    public static class StringExtension
    {
        public static byte[] GetBytes(this string conteudo)
            => Encoding.UTF8.GetBytes(conteudo);
    }
}