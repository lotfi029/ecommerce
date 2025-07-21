using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using InventoryService.Core.Entities;
using InventoryService.Core.Features.Inventories.Commands.Update;
using InventoryService.Core.IRepositories;
using eCommerce.SharedKernal.Messaging;

public class UpdateInventoryCommandHandlerTests
{
    private readonly Mock<IInventoryRepository> _repoMock = new();
    private readonly UpdateInventoryCommandHandler _handler;

    public UpdateInventoryCommandHandlerTests()
    {
        _handler = new UpdateInventoryCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenInventoryDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Inventory)null!);
        var cmd = new UpdateInventoryCommand("user", Guid.NewGuid(), 1, true);
        var result = await _handler.HandleAsync(cmd);
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task HandleAsync_ReturnsInsufficientStock_WhenReservationExceedsAvailable()
    {
        var inventory = new Inventory { ProductId = Guid.NewGuid(), Quantity = 5, Reserved = 4 };
        _repoMock.Setup(r => r.GetByIdAsync(inventory.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventory);
        var cmd = new UpdateInventoryCommand("user", inventory.ProductId, 2, true);
        var result = await _handler.HandleAsync(cmd);
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.InsufficientStock", result.Error.Code);
    }

    [Fact]
    public async Task HandleAsync_ReservesStock_WhenSufficientAvailable()
    {
        var inventory = new Inventory { ProductId = Guid.NewGuid(), Quantity = 10, Reserved = 2 };
        _repoMock.Setup(r => r.GetByIdAsync(inventory.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventory);
        _repoMock.Setup(r => r.UpdateAsync(inventory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var cmd = new UpdateInventoryCommand("user", inventory.ProductId, 5, true);
        var result = await _handler.HandleAsync(cmd);
        Assert.True(result.IsSuccess);
        Assert.Equal(7, inventory.Reserved);
    }

    [Fact]
    public async Task HandleAsync_ReturnsInsufficientStock_WhenQuantityUpdateBelowReserved()
    {
        var inventory = new Inventory { ProductId = Guid.NewGuid(), Quantity = 5, Reserved = 4 };
        _repoMock.Setup(r => r.GetByIdAsync(inventory.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventory);
        var cmd = new UpdateInventoryCommand("user", inventory.ProductId, -2, false);
        var result = await _handler.HandleAsync(cmd);
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.InsufficientStock", result.Error.Code);
    }

    [Fact]
    public async Task HandleAsync_UpdatesQuantity_WhenValid()
    {
        var inventory = new Inventory { ProductId = Guid.NewGuid(), Quantity = 10, Reserved = 2 };
        _repoMock.Setup(r => r.GetByIdAsync(inventory.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventory);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Inventory>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var cmd = new UpdateInventoryCommand("user", inventory.ProductId, 5, false);
        var result = await _handler.HandleAsync(cmd);
        Assert.True(result.IsSuccess);
        Assert.Equal(15, inventory.Quantity);
    }
}
