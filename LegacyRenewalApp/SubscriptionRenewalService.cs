using System;
using LegacyRenewalApp.Refactor;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            InputValidator.ValidateInput(customerId, planCode, seatCount, paymentMethod);

            string normalizedPlanCode = Normalizer.Normalize(planCode);
            string normalizedPaymentMethod = Normalizer.Normalize(paymentMethod);
            
            var (customer, plan) = InputValidator.RetrieveAndValidateCustomer(customerId, normalizedPlanCode);

            Discount discount = new Discount((plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee);

            customer.ApplyDiscountBasedOnPlan(discount, plan);
            customer.ApplyDicountBasedOnYearsWithCompany(discount);
            customer.ApplyDiscountBasedOnLoyaltyPoints(discount, useLoyaltyPoints);

            if (seatCount >= 50)
            {
                discount.AddDisc(0.12m, "large team discount; ");
            }
            else if (seatCount >= 20)
            {
                discount.AddDisc(0.08m, "medium team discount; ");
            }
            else if (seatCount >= 10)
            {
                discount.AddDisc(0.04m, "small team discount; ");
            }
            
            discount.CalculateSubtotal();
            discount.CalculateSupportFee(includePremiumSupport, normalizedPlanCode);
            discount.CalculatePaymentFee(normalizedPaymentMethod);

            Tax tax = new Tax(customer, discount);

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(discount.baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discount.discountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(discount.supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(discount.paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(tax.taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(tax.finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = discount.notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            IBillingGateWay billingGateWay = new LegacyBillingGatewayWrapper();
            
            billingGateWay.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                billingGateWay.SendNotification(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
