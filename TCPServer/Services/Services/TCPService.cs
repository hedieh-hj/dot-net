using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerApi.Services
{
    public class TCPService
    {
        public async Task<bool> GetData(ValidationViewModel model)
        {
            var client = new TcpClient();

            try
            {
                await client.ConnectAsync("127.0.0.1", 63200);

                var _network_stream = client.GetStream();

                var json = System.Text.Json.JsonSerializer.Serialize(model) + "\n";

                var bytesToSend = Encoding.ASCII.GetBytes(json);

                await _network_stream.WriteAsync(bytesToSend);

                await _network_stream.FlushAsync();

                var buffer = new byte[1024];

                var byteCount = await _network_stream.ReadAsync(buffer);

                var _response = Encoding.ASCII.GetString(buffer, 0, byteCount).Trim();

                _network_stream.Close();

                await _network_stream.DisposeAsync();

                client.Close();

                return _response.Equals("true", StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}

