using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoUnitTests.UnitTests
{
    public class PostProdutosUnitTests : IClassFixture<ProdutoUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PostProdutosUnitTests(ProdutoUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PostProduto_ReturnsCreatedAtActionResult()
        {
            //Arrange
            var newProduto = new APICatalogo.DTO.ProdutoDTO
            {
                Nome = "Produto Teste",
                Descricao = "Descrição do Produto Teste",
                Preco = 9.99m,
                UrlImagem = "imagem.jpg",
                IdCategoria = 1
            };

            //Act
            var data = await _controller.Post(newProduto);

            //Assert
            data.Result.Should().BeOfType<CreatedAtRouteResult>()
                .Which.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task PostProduto_Return_BadRequest()
        {
            ProdutoDTO produto = null;

            var data = await _controller.Post(produto);

            data.Result.Should().BeOfType<BadRequestResult>()
                .Which.StatusCode.Should().Be(400);
        }
    }
}
