using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MPService.Infrastructure.Services
{
    public class CloudflaredTunnelService : BackgroundService
    {
        private readonly ILogger<CloudflaredTunnelService> _logger;
        private Process _process;

        public CloudflaredTunnelService(ILogger<CloudflaredTunnelService> logger)
        {
            _logger = logger;

            // Fallback shutdown hook if Visual Studio force-quits
            AppDomain.CurrentDomain.ProcessExit += (_, __) => KillCloudflared();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cloudflared tunnel service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cloudflared",
                            Arguments = "tunnel run your-tunnel-name",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        },
                        EnableRaisingEvents = true
                    };

                    _process.Start();
                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();

                    _logger.LogInformation("cloudflared started with PID {pid}", _process.Id);

                    while (!_process.HasExited && !stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }

                    if (!_process.HasExited)
                    {
                        KillCloudflared();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "cloudflared crashed or failed to start.");
                    await Task.Delay(5000, stoppingToken); // Retry delay
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync called.");
            KillCloudflared();
            return base.StopAsync(cancellationToken);
        }

        public void Dispose()
        {
            _logger.LogInformation("Dispose called.");
            KillCloudflared();
        }

        private void KillCloudflared()
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _logger.LogInformation("Killing cloudflared process...");
                    _process.Kill(entireProcessTree: true);
                    _process.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to kill cloudflared.");
            }
        }
    }
}
