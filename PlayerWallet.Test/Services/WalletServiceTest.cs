using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using PlayerWallet.Data.Models;
using PlayerWallet.Data.Repositories;
using PlayerWallet.Data.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PlayerWallet.Test.Services
{
    public class WalletServiceTest
    {
        private IWalletService walletService;

        private readonly Mock<IWalletRepository> _walletRepositoryMock;

        private readonly List<Wallet> fakeWallets = new List<Wallet>()
        {
            new Wallet { Guid = Guid.Parse("00000000-0000-0000-0000-000000000001"), Balance = 0},
            new Wallet { Guid = Guid.Parse("00000000-0000-0000-0000-000000000002"), Balance = 100 }
        };
        
        public WalletServiceTest()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            foreach (var fakeWallet in fakeWallets)
            {
                _walletRepositoryMock.Setup(r => r.GetByGuidAsync(fakeWallet.Guid)).ReturnsAsync(fakeWallet);
            }
            
            walletService = new WalletService(_walletRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_NonExistingGuid_AddsToRepo()
        {
            var nonExistingGuid = Guid.NewGuid();

            _walletRepositoryMock.Setup(r => r.GetByGuidAsync(nonExistingGuid)).Returns(Task.FromResult<Wallet>(null));

            await walletService.AddAsync(nonExistingGuid);

            _walletRepositoryMock.Verify(r => r.AddAsync(nonExistingGuid), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ExistingGuid_ThrowsError()
        {
            var existingGuid = fakeWallets[0].Guid;

            _walletRepositoryMock.Setup(r => r.GetByGuidAsync(existingGuid)).ReturnsAsync(fakeWallets[0]);

            try
            {
                await walletService.AddAsync(existingGuid);
            }
            catch (ArgumentException)
            {
            }

            _walletRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task GetByGuidAsync_CallsRepoWithCorrectArgument()
        {
            var fakeGuid = Guid.NewGuid();

            await walletService.GetByGuidAsync(fakeGuid);

            _walletRepositoryMock.Verify(r => r.GetByGuidAsync(fakeGuid), Times.Once);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000001", 0)]
        [InlineData("00000000-0000-0000-0000-000000000002", 100)]
        public async Task GetBalanceAsync_CorrectArguments_ReturnsCorrect(string stringGuid, int expectedValue)
        {
            var fakeGuid = Guid.Parse(stringGuid);

            var actual = await walletService.GetBalanceAsync(fakeGuid);

            actual.Should().Equals(expectedValue);
        }

        [Fact]
        public async Task GetBalanceAsync_NotExisting_ThrowsError()
        {

            var nonExistingGuid = Guid.NewGuid();
            _walletRepositoryMock.Setup(r => r.GetByGuidAsync(nonExistingGuid)).Returns(Task.FromResult<Wallet>(null));

            var actualEx = await Record.ExceptionAsync(async () => await walletService.GetBalanceAsync(nonExistingGuid));

            using (new AssertionScope())
            {
                actualEx.Should().BeOfType(typeof(ArgumentException));
                actualEx.Message.Should().Be("Wallet Not Found");
            }
        }

        [Fact]
        public async Task UpdateAsync_CallsRepo()
        {
            var fakeWallet = new Wallet() { Guid = Guid.NewGuid(), Balance = 0 };

            await walletService.UpdateAsync(fakeWallet);

            _walletRepositoryMock.Verify(r => r.UpdateAsync(fakeWallet), Times.Once);
        }
    }
}
