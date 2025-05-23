using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MPService.Infrastructure.Services
{
    public class CloudflaredTunnelService : BackgroundService
    {
        private readonly ILogger<CloudflaredTunnelService> _logger;

        public CloudflaredTunnelService(ILogger<CloudflaredTunnelService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cloudflared tunnel service is starting.");

            var exePath = Path.Combine(AppContext.BaseDirectory, "Tools", "cloudflared.exe");

            if (!File.Exists(exePath))
            {
                _logger.LogError("cloudflared.exe not found at: {Path}", exePath);
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "tunnel run malayanprints-tunnel", // script
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logger.LogInformation("cloudflared: {Message}", e.Data);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logger.LogError("cloudflared ERROR: {Message}", e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            _logger.LogInformation("cloudflared started. PID: {Pid}", process.Id);

            try
            {
                await process.WaitForExitAsync(stoppingToken);
                _logger.LogWarning("cloudflared exited with code: {ExitCode}", process.ExitCode);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("cloudflared task was cancelled.");
                if (!process.HasExited)
                {
                    _logger.LogInformation("Killing cloudflared process...");
                    process.Kill(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while running cloudflared.");
            }
        }

        
    }
}
