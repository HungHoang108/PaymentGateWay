using PaymentGateWay.Contracts;
using PaymentGateWay.Models.Stripe;
using Stripe;

namespace PaymentGateWay.Application
{
    public class StripeAppService : IStripeAppService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly CardService _cardService;

        public StripeAppService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            CardService cardService)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _cardService = cardService;
        }

        public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
        {
            CustomerCreateOptions customerOptions = new CustomerCreateOptions
            {
                Name = customer.Name,
                Email = customer.Email,
            };
            Customer stripeCustomer = await _customerService.CreateAsync(customerOptions, null, ct);
            var cardTokenOptions = new CardCreateOptions
            {
                Source = "tok_visa_debit",
            };
            var cardToken = await _cardService.CreateAsync(stripeCustomer.Id.ToString(), cardTokenOptions, null, ct);
            return new StripeCustomer(stripeCustomer.Name, stripeCustomer.Email, stripeCustomer.Id);

            //TokenCreateOptions tokenOptions = new TokenCreateOptions
            //{
            //    Card = new TokenCardOptions
            //    {
            //        Name = customer.Name,
            //        Number = customer.CreditCard.CardNumber,
            //        ExpYear = customer.CreditCard.ExpirationYear,
            //        ExpMonth = customer.CreditCard.ExpirationMonth,
            //        Cvc = customer.CreditCard.Cvc
            //    }
            //};

            //Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);

            //CustomerCreateOptions customerOptions = new CustomerCreateOptions
            //{
            //    Name = customer.Name,
            //    Email = customer.Email,
            //    Source = stripeToken.Id
            //};

            //Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);

            //return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
        }
        public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct)
        {
            ChargeCreateOptions paymentOptions = new ChargeCreateOptions
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = payment.Amount
            };

            var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);

            return new StripePayment(
              createdPayment.CustomerId,
              createdPayment.ReceiptEmail,
              createdPayment.Description,
              createdPayment.Currency,
              createdPayment.Amount,
              createdPayment.Id);
        }
    }

}
