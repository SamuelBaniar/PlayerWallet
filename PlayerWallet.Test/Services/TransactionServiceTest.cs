using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using PlayerWallet.Data.Internal;
using PlayerWallet.Data.Models;
using PlayerWallet.Data.Repositories;
using PlayerWallet.Data.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PlayerWallet.Test.Services
{
    public class TransactionServiceTest
    {
        private ITransactionService transactionService;

        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IWalletService> _walletServiceMock;

        public TransactionServiceTest()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _walletServiceMock = new Mock<IWalletService>();

            transactionService = new TransactionService(_transactionRepositoryMock.Object, _walletServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_InvalidWalletGuid_ReturnsSuccessFalse()
        {
            var fakeTransaction = new WalletTransaction() { Guid = Guid.NewGuid(), Amount = 1, TransactionType = TransactionType.Deposit, WalletGuid = Guid.NewGuid() };

            _walletServiceMock.Setup(s => s.GetByGuidAsync(fakeTransaction.WalletGuid)).Returns(Task.FromResult<Wallet>(null));

            var result = await transactionService.AddAsync(fakeTransaction);

            using (new AssertionScope())
            {
                result.Sucess.Should().BeFalse();
                result.Message.Should().Be("Transaction for invalid Wallet");

                _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WalletTransaction>()), Times.Never);
                _walletServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Wallet>()), Times.Never);
            }
        }

        [Fact]
        public async Task AddAsync_AlreadyProcessedTransaction_ReturnsSuccessTrue()
        {
            var fakeTransaction = new WalletTransaction() { Guid = Guid.NewGuid(), Amount = 1, TransactionType = TransactionType.Deposit, WalletGuid = Guid.NewGuid() };

            _walletServiceMock.Setup(s => s.GetByGuidAsync(fakeTransaction.WalletGuid)).ReturnsAsync(new Wallet());
            _transactionRepositoryMock.Setup(r => r.GetByGuidAsync(fakeTransaction.Guid)).ReturnsAsync(fakeTransaction);

            var result = await transactionService.AddAsync(fakeTransaction);

            using (new AssertionScope())
            {
                result.Sucess.Should().BeTrue();
                result.Message.Should().Be("Transaction was already processed.");

                _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WalletTransaction>()), Times.Never);
                _walletServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Wallet>()), Times.Never);
            }
        }

        [Fact]
        public async Task AddAsync_InvalidTransactionType_ReturnsSuccessFalse()
        {
            var fakeTransaction = new WalletTransaction() { Guid = Guid.NewGuid(), Amount = 1, TransactionType = "Unknown", WalletGuid = Guid.NewGuid() };

            _walletServiceMock.Setup(s => s.GetByGuidAsync(fakeTransaction.WalletGuid)).ReturnsAsync(new Wallet());
            _transactionRepositoryMock.Setup(r => r.GetByGuidAsync(fakeTransaction.Guid)).Returns(Task.FromResult<WalletTransaction>(null));

            var result = await transactionService.AddAsync(fakeTransaction);

            using (new AssertionScope())
            {
                result.Sucess.Should().BeFalse();

                _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WalletTransaction>()), Times.Never);
                _walletServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Wallet>()), Times.Never);
            }
        }

        [Fact]
        public async Task AddAsync_InsufficientFunds_ReturnsSuccessFalse()
        {
            var fakeWallet = new Wallet() { Guid = Guid.NewGuid(), Balance = 99 };
            var fakeTransaction = new WalletTransaction() { Guid = Guid.NewGuid(), Amount = 100, TransactionType = TransactionType.Stake, WalletGuid = fakeWallet.Guid };

            _walletServiceMock.Setup(s => s.GetByGuidAsync(fakeTransaction.WalletGuid)).ReturnsAsync(fakeWallet);
            _transactionRepositoryMock.Setup(r => r.GetByGuidAsync(fakeTransaction.Guid)).Returns(Task.FromResult<WalletTransaction>(null));

            var result = await transactionService.AddAsync(fakeTransaction);

            using (new AssertionScope())
            {
                result.Sucess.Should().BeFalse();
                result.Message.Should().Be("Not enough gold.");

                _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<WalletTransaction>()), Times.Never);
                _walletServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Wallet>()), Times.Never);
            }
        }

        [Fact]
        public async Task AddAsync_CorrectTransaction_ReturnsSuccessTrueAndSaves()
        {
            var fakeWallet = new Wallet() { Guid = Guid.NewGuid(), Balance = 101 };
            var fakeTransaction = new WalletTransaction() { Guid = Guid.NewGuid(), Amount = 100, TransactionType = TransactionType.Stake, WalletGuid = fakeWallet.Guid };

            _walletServiceMock.Setup(s => s.GetByGuidAsync(fakeTransaction.WalletGuid)).ReturnsAsync(fakeWallet);
            _transactionRepositoryMock.Setup(r => r.GetByGuidAsync(fakeTransaction.Guid)).Returns(Task.FromResult<WalletTransaction>(null));

            var result = await transactionService.AddAsync(fakeTransaction);

            using (new AssertionScope())
            {
                result.Sucess.Should().BeTrue();

                _transactionRepositoryMock.Verify(r => r.AddAsync(fakeTransaction), Times.Once);
                _walletServiceMock.Verify(s => s.UpdateAsync(It.Is<Wallet>(i => i.Guid == fakeWallet.Guid && i.Balance == 1)), Times.Once);
            }
        }
    }
}
