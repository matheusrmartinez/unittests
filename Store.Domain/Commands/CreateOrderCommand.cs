using System.Collections.Generic;
using Flunt.Notifications;
using Flunt.Validations;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Utils;

namespace Store.Domain.Commands
{
    public class CreateOrderCommand : Notifiable, ICommand
    {
        public CreateOrderCommand()
        {
            Items = new List<CreateOrderItemCommand>();
        }

        public CreateOrderCommand(string customer, string zipCode, string promoCode, IList<CreateOrderItemCommand> items)
        {
            Customer = customer;
            ZipCode = zipCode;
            PromoCode = promoCode;
            Items = items;
        }

        public string Customer { get; set; }
        public string ZipCode { get; set; }
        public string PromoCode { get; set; }
        public IList<CreateOrderItemCommand> Items { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
            .Requires()
            .HasLen(Customer, 11, "Customer", "Cliente inválido")
            .HasLen(ZipCode, 8, "ZipCode", "CEP inválido")
            .IsTrue(Items.Count == 0, "Items", "Nenhum item de pedido foi selecionado")
            );
        }
    }
}