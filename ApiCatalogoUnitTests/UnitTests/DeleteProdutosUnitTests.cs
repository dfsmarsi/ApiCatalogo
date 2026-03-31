using APICatalogo.Controllers;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace ApiCatalogoUnitTests.UnitTests
{
    // ============================================================
    // Testes COM Mock — isola dependências externas
    // ============================================================
    public class DeleteProdutosMockTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _uofMock;
        private readonly ProdutosController _controller;

        public DeleteProdutosMockTests(ProdutoUnitTestController fixture)
        {
            _mapper = fixture.mapper;
            _uofMock = new Mock<IUnitOfWork>();
            _controller = new ProdutosController(_uofMock.Object, _mapper);
        }

        [Fact]
        public async Task DeleteProduto_Returns_OkResult_WhenDeleteSucceeds()
        {
            // Arrange
            var id = 1;
            var produto = new Produto
            {
                IdProduto = 1,
                Nome = "Produto Teste",
                Descricao = "Descrição Teste",
                Preco = 10.00m,
                IdCategoria = 1
            };

            _uofMock.Setup(u => u.ProdutoRepository.GetIdAsync(It.IsAny<Expression<Func<Produto, bool>>>()))
                    .ReturnsAsync(produto);

            _uofMock.Setup(u => u.ProdutoRepository.Delete(produto))
                    .Returns(produto);

            _uofMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            var data = await _controller.Delete(id);

            // Assert
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

            data.Result.As<OkObjectResult>().Value
                .Should().BeOfType<ProdutoDTO>()
                .Which.IdProduto.Should().Be(id);
        }

        [Fact]
        public async Task DeleteProduto_Returns_InternalServerError_WhenProductNotFound()
        {
            // Arrange
            var id = 9999;

            _uofMock.Setup(u => u.ProdutoRepository.GetIdAsync(It.IsAny<Expression<Func<Produto, bool>>>()))
                    .ReturnsAsync((Produto)null);

            // Act
            var data = await _controller.Delete(id);

            // Assert
            data.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteProduto_Calls_Repository_Delete_Once()
        {
            // Arrange
            var id = 1;
            var produto = new Produto { IdProduto = 1, Nome = "Produto Teste", IdCategoria = 1 };

            _uofMock.Setup(u => u.ProdutoRepository.GetIdAsync(It.IsAny<Expression<Func<Produto, bool>>>()))
                    .ReturnsAsync(produto);

            _uofMock.Setup(u => u.ProdutoRepository.Delete(produto))
                    .Returns(produto);

            _uofMock.Setup(u => u.CommitAsync())
                    .Returns(Task.CompletedTask);

            // Act
            await _controller.Delete(id);

            // Assert
            _uofMock.Verify(u => u.ProdutoRepository.Delete(produto), Times.Once);
            _uofMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProduto_Does_Not_Call_Delete_WhenProductNotFound()
        {
            // Arrange
            var id = 9999;

            _uofMock.Setup(u => u.ProdutoRepository.GetIdAsync(It.IsAny<Expression<Func<Produto, bool>>>()))
                    .ReturnsAsync((Produto)null);

            // Act
            await _controller.Delete(id);

            // Assert — Delete e CommitAsync nunca devem ser chamados
            _uofMock.Verify(u => u.ProdutoRepository.Delete(It.IsAny<Produto>()), Times.Never);
            _uofMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
