using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;

namespace TravelMateAPI.Services.Auth
{         // cái này dùng để xóa tài khoản khi trong 30 phút người dùng không vào xác nhận email

    public class AccountCleanupService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public AccountCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DeleteUnconfirmedAccounts, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private async void DeleteUnconfirmedAccounts(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var users = userManager.Users;

                foreach (var user in users)
                {
                    // Kiểm tra nếu tài khoản chưa được xác nhận và đã quá 30 phút
                    if (!user.EmailConfirmed && user.RegistrationTime.AddMinutes(30) < DateTime.UtcNow)
                    {
                        await userManager.DeleteAsync(user);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
