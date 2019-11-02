using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store.Domain.Commands;
using Store.Domain.Entities;
using Store.Domain.Handlers;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Utils;
using Store.Tests.Repositories;

namespace Store.Tests.Handler
{
    [TestClass]
    public class OrderHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderHandlerTests()
        {
            _customerRepository = new FakeCustomerRepository();
            _deliveryFeeRepository = new FakeDeliveryFeeRepository();
            _discountRepository = new FakeDiscountRepository();
            _orderRepository = new FakeOrderRepository();
            _productRepository = new FakeProductRepository();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
        {
            var createOrderCommand = new CreateOrderCommand();
            createOrderCommand.Customer = "12345678912";
            createOrderCommand.ZipCode = "12345678";
            createOrderCommand.PromoCode = string.Empty;
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var handler = new OrderHandlers(
                _customerRepository,
                _deliveryFeeRepository,
                _discountRepository,
                _productRepository,
                _orderRepository
            );

            handler.Handle(createOrderCommand);
            Assert.AreEqual(handler.Valid, false);
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_cep_invalido_o_pedido_nao_deve_ser_gerado()
        {
            // var createOrderCommand = new CreateOrderCommand();
            // createOrderCommand.Customer = "12345678911";
            // createOrderCommand.ZipCode = "12345678";
            // createOrderCommand.PromoCode = string.Empty;
            // createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            // createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            // var handler = new OrderHandlers(
            //     _customerRepository,
            //     _deliveryFeeRepository,
            //     _discountRepository,
            //     _productRepository,
            //     _orderRepository
            // );

            // handler.Handle(createOrderCommand);
            // Assert.AreEqual(handler.Valid, false);
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_promocode_inexistente_o_pedido_deve_ser_gerado_normalmente()
        {
            var createOrderCommand = new CreateOrderCommand();
            createOrderCommand.Customer = "12345678911";
            createOrderCommand.ZipCode = "12345678";
            createOrderCommand.PromoCode = string.Empty;
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var handler = new OrderHandlers(
                _customerRepository,
                _deliveryFeeRepository,
                _discountRepository,
                _productRepository,
                _orderRepository
            );

            handler.Handle(createOrderCommand);
            Assert.AreEqual(handler.Valid, true);
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_pedido_sem_itens_o_mesmo_nao_deve_ser_gerado()
        {
            // var createOrderCommand = new CreateOrderCommand();
            // createOrderCommand.Customer = "12345678911";
            // createOrderCommand.ZipCode = "12345678";
            // createOrderCommand.PromoCode = string.Empty;

            // var handler = new OrderHandlers(
            //     _customerRepository,
            //     _deliveryFeeRepository,
            //     _discountRepository,
            //     _productRepository,
            //     _orderRepository
            // );

            // handler.Handle(createOrderCommand);
            // Assert.AreEqual(createOrderCommand.Valid, false);
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_comando_invalido_o_pedido_nao_deve_ser_gerado()
        {
            // var createOrderCommand = new CreateOrderCommand();
            // createOrderCommand.Customer = "123456789112";
            // createOrderCommand.ZipCode = "12345678";
            // createOrderCommand.PromoCode = string.Empty;
            // createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            // createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            // var handler = new OrderHandlers(
            //     _customerRepository,
            //     _deliveryFeeRepository,
            //     _discountRepository,
            //     _productRepository,
            //     _orderRepository
            // );

            // handler.Handle(createOrderCommand);
            // Assert.AreEqual(createOrderCommand.Valid, false);
        }

        [TestMethod]
        [TestCategory("Handlers")]
        public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
        {
            var createOrderCommand = new CreateOrderCommand();
            createOrderCommand.Customer = "12345678911";
            createOrderCommand.ZipCode = "12345678";
            createOrderCommand.PromoCode = string.Empty;
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var handler = new OrderHandlers(
                _customerRepository,
                _deliveryFeeRepository,
                _discountRepository,
                _productRepository,
                _orderRepository
            );

            handler.Handle(createOrderCommand);
            Assert.AreEqual(handler.Valid, true);
        }
    }
}