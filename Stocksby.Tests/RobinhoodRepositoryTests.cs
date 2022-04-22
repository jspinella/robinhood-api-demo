using Stocksby.Repositories;
using Xunit;
using System;

namespace Stocksby.Tests
{
    public class RobinhoodRepositoryTests
    {
        [Fact]
        public async void GetBearerToken_Base()
        {
            var username = "";
            var password = "";

            var result = await RobinhoodRepository.GetBearerToken(username, password);

            Assert.NotEmpty(result.token);
            Assert.True(result.expirationDate > DateTime.Now);
        }

        [Fact]
        public async void GetQuote_Base()
        {
            var username = "";
            var password = "";

            var result = await RobinhoodRepository.GetBearerToken(username, password);

            Assert.NotEmpty(result.token);
            Assert.True(result.expirationDate > DateTime.Now);
        }
    }
}