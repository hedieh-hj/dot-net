using Services.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace TCPAgent
{
    public class ValidationService
    {
        private TcpListener? _listener;
        private readonly int _port = 63200;

        public async Task ListenToClientAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Run(async () =>
            {
                _listener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), _port);
                
                _listener.Start();

                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();

                    try
                    {
                        var _network_stream = client.GetStream();

                        var reader = new StreamReader(_network_stream, Encoding.ASCII);

                        var writer = new StreamWriter(_network_stream, Encoding.ASCII) { AutoFlush = true };

                        var json = await reader.ReadLineAsync();

                        var val_model = JsonSerializer.Deserialize<ValidationViewModel>(json);

                        var result = await ValidationAsync(val_model);

                        await writer.WriteLineAsync(result ? "true" : "false");

                        _network_stream.Close();

                        await _network_stream.DisposeAsync();

                        client.Close();

                        client.Dispose();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }, cancellationToken);
        }

        private async Task<bool> ValidationAsync(ValidationViewModel model)
        {
            try
            {
                //here paste your code 
                //http request 

                if(model.TaxID.Length % 2 == 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}