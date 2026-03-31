using APICatalogo.Controllers;
using APICatalogo.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoUnitTests.UnitTests
{
    public class GetProdutosUnitTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly ProdutosController _controller;

        public GetProdutosUnitTests(ProdutoUnitTestController fixture)
        {
            _controller = new ProdutosController(fixture.repository, fixture.mapper);
        }

        [Fact]
        public async Task GetProdutosById_ReturnsOkResult()
        {
            var produtoId = 10;
            var data = await _controller.GetProdutoPorId(produtoId);
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetProdutosById_ReturnsNotFoundResult()
        {
            var produtoId = 9999;
            var data = await _controller.GetProdutoPorId(produtoId);
            data.Result.Should().BeOfType<NotFoundResult>()
                .Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProdutosById_ReturnsBadRequest()
        {
            var produtoId = -1;
            var data = await _controller.GetProdutoPorId(produtoId);
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProdutos_Returns_ListOfProdutosDTO()
        {
            var data = await _controller.GetTodosProdutos();
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<List<APICatalogo.DTO.ProdutoDTO>>()
                .And.NotBeNull();
        }
    }

    // ============================================================
    // Testes COM Mock — isola dependências externas
    // ============================================================
    public class GetProdutosMockTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _uofMock;
        private readonly ProdutosController _controller;

        public GetProdutosMockTests(ProdutoUnitTestController fixture)
        {
            _mapper = fixture.mapper;
            _uofMock = new Mock<IUnitOfWork>();
            _controller = new ProdutosController(_uofMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProdutos_Returns_BadRequest_WhenRepositoryThrows()
        {
            // Arrange
            _uofMock.Setup(u => u.ProdutoRepository.GetAllAsync())
                    .ThrowsAsync(new Exception("Erro simulado"));

            // Act
            var data = await _controller.GetTodosProdutos();

            // Assert
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                        .Which.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProdutos_Returns_NotFound_WhenRepositoryReturnsNull()
        {
            // Arrange
            _uofMock.Setup(u => u.ProdutoRepository.GetAllAsync())
                    .ReturnsAsync((IEnumerable<APICatalogo.Models.Produto>)null);

            // Act
            var data = await _controller.GetTodosProdutos();

            // Assert
            data.Result.Should().BeOfType<NotFoundResult>()
                        .Which.StatusCode.Should().Be(404);
        }
    }
}
