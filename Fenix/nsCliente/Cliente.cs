using System.Text.Json.Serialization;

namespace Fenix.nsCliente
{
    public class Cliente
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("documento")]
        public string Documento { get; set; }
    }
}