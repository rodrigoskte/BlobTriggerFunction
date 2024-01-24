using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobTriggerExemplo
    {
        private readonly ILogger<BlobTriggerExemplo> _logger;
        private const string PASTA_DE_TRABALHO = @"F:\Git\BlobTriggerFunction\blob.txt";
        private const int TAMANHO_DO_ARQUIVO = 2048;
        private const int CARACTER_INICIAL_LEITURA_ARQUIVO = 0;

        public BlobTriggerExemplo(ILogger<BlobTriggerExemplo> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobTriggerExemplo))]
        public async Task Run([BlobTrigger("samples-workitems/{name}", Connection = "blobtriggerexemplofunc_STORAGE")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");

            var escritor = new StreamWriter(PASTA_DE_TRABALHO);
            var conteudo = new byte[TAMANHO_DO_ARQUIVO];

            escritor.WriteLine($"\nBlob adicionado: {name}");

            stream.Read(
                conteudo, 
                CARACTER_INICIAL_LEITURA_ARQUIVO,
                conteudo.Length);

            escritor.WriteLine(
                Encoding.UTF8.GetString(
                    conteudo, 
                    CARACTER_INICIAL_LEITURA_ARQUIVO,
                    conteudo.Length)
                    );
                    
            escritor.Close();    
        }
    }
}
