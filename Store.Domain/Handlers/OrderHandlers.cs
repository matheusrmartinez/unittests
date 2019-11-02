using System.Linq;
using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    public class OrderHandlers : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderHandlers
        (
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return new GenericCommandResult(false, "Pedido inválido", command.Notifications);

            // 1. Recupera o cliente
            var customer = _customerRepository.Get(command.Customer);

            // 2. Calcula a taxa de entrega
            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

            // 3. Obtém o cupom de desconto
            var discount = _discountRepository.Get(command.PromoCode);

            if (command.Items.Count == 0)
                return new GenericCommandResult(false, "Não foram selecionados itens de pedido", command.Notifications);

            // 4. Gera o pedido
            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            var order = new Order(customer, deliveryFee, discount);
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }

            // 5. Agrupa as notificações
            AddNotifications(order.Notifications);

            // 6. Verifica se não adicionou o pedido corretamente
            if (Invalid)
                return new GenericCommandResult(false, "Falha ao gerar o pedido", Notifications);

            // 7. Retorna o resultado
            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.Number} inserido com sucesso", order);

        }
    }
}