using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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

        public GetProdutosUnitTests(ProdutoUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutosById_ReturnsOkResult()
        {
            //Arrange
            var produtoId = 2;

            // Act
            var data = await _controller.GetProdutoPorId(produtoId);

            // Assert
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //FluentAssertions
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }
    }
}
