using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PictureLibrary_API.Helpers;
using PictureLibrary_API.Model;
using PictureLibrary_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace PictureLibrary_API.Tests.ServicesTests
{
    public class AccessTokenServiceTests
    {
        #region Private helper methods
        private RefreshToken GetRefreshTokenSample()
        {
            return new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Token = "0R1NN3ErZDCv2ht0XU7WEo/INcbDC9/Z4G8kWeX81yA="
            };

        }
        #endregion

        #region DeleteRefreshTokenAsync
        [Fact]
        public async void DeleteRefreshTokenAsync_ShouldRemoveRefreshTokenFromDatabase()
        {
            var loggerMock = new Mock<ILogger<AccessTokenService>>();
            var optionsMock = new Mock<IOptions<AppSettings>>();
            var appSettingsMock = new Mock<AppSettings>();
            var contextMock = new Mock<IDatabaseContext>();

            optionsMock.Setup(x => x.Value)
                .Returns(appSettingsMock.Object);

            var refreshToken = GetRefreshTokenSample();
            var dbSet = new TestDbSet<RefreshToken>();
            dbSet.Add(refreshToken);

            contextMock.Setup(x => x.RefreshTokens)
                .Returns(dbSet);
            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            var service = new AccessTokenService(loggerMock.Object, contextMock.Object, optionsMock.Object);

            await service.DeleteRefreshTokenAsync(refreshToken.UserId.ToString(), refreshToken.Token);

            Assert.DoesNotContain(refreshToken, dbSet);
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
        #endregion
    }
}
