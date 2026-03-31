using APICatalogo.Controllers;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiCatalogoUnitTests.UnitTests
{
    public class PutProdutosUnitTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutosUnitTests(ProdutoUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Returns_BadRequest_WhenIdMismatch()
        {
            // Arrange
            var id = 1;
            var produtoDTO = new ProdutoDTO
            {
                IdProduto = 99, // ID diferente do parâmetro da rota
                Nome = "Produto Teste",
                Descricao = "Descrição Teste",
                Preco = 10.00m,
                IdCategoria = 1
            };

            // Act
            var data = await _controller.Put(id, produtoDTO);

            // Assert
            data.Result.Should().BeOfType<BadRequestResult>()
                .Which.StatusCode.Should().Be(400);
        }
    }

    // ============================================================
    // Testes COM Mock — isola dependências externas
    // ============================================================
    public class PutProdutosMockTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _uofMock;
        private readonly ProdutosController _controller;

        public PutProdutosMockTests(ProdutoUnitTestController fixture)
        {
            _mapper = fixture.mapper;
            _uofMock = new Mock<IUnitOfWork>();
            _controller = new ProdutosController(_uofMock.Object, _mapper);
        }

        [Fact]
        public async Task PutProduto_Returns_OkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var id = 1;
            var produtoDTO = new ProdutoDTO
            {
                IdProduto = 1,
                Nome = "Produto Atualizado",
                Descricao = "Descrição Atualizada",
                Preco = 29.99m,
                IdCategoria = 1
            };

            var produtoAtualizado = new Produto
            {
                IdProduto = 1,
                Nome = "Produto Atualizado",
                Descricao = "Descrição Atualizada",
                Preco = 29.99m,
                IdCategoria = 1
            };

            _uofMock.Setup(u => u.ProdutoRepository.Update(It.IsAny<Produto>()))
                    .Returns(produtoAtualizado);

            _uofMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            var data = await _controller.Put(id, produtoDTO);

            // Assert
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

            data.Result.As<OkObjectResult>().Value
                .Should().BeOfType<ProdutoDTO>()
                .Which.IdProduto.Should().Be(id);
        }

        [Fact]
        public async Task PutProduto_Returns_InternalServerError_WhenRepositoryReturnsNull()
        {
            // Arrange
            var id = 1;
            var produtoDTO = new ProdutoDTO
            {
                IdProduto = 1,
                Nome = "Produto Teste",
                Descricao = "Descrição Teste",
                Preco = 10.00m,
                IdCategoria = 1
            };

            _uofMock.Setup(u => u.ProdutoRepository.Update(It.IsAny<Produto>()))
                    .Returns((Produto)null);

            _uofMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            var data = await _controller.Put(id, produtoDTO);

            // Assert
            data.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task PutProduto_Calls_Repository_Update_Once()
        {
            // Arrange
            var id = 1;
            var produtoDTO = new ProdutoDTO
            {
                IdProduto = 1,
                Nome = "Produto Teste",
                Descricao = "Descrição Teste",
                Preco = 10.00m,
                IdCategoria = 1
            };

            var produtoAtualizado = new Produto { IdProduto = 1, Nome = "Produto Teste", IdCategoria = 1 };

            _uofMock.Setup(u => u.ProdutoRepository.Update(It.IsAny<Produto>()))
                    .Returns(produtoAtualizado);

            _uofMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            await _controller.Put(id, produtoDTO);

            // Assert
            _uofMock.Verify(u => u.ProdutoRepository.Update(It.IsAny<Produto>()), Times.Once);
            _uofMock.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
