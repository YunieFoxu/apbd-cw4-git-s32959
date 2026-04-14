using System;

namespace LegacyRenewalApp.Refactor;

public class InputValidator
{
    public static void ValidateInput(int customerId, string planCode, int seatCount, string paymentMethod)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer id must be positive");
        }

        if (string.IsNullOrWhiteSpace(planCode))
        {
            throw new ArgumentException("Plan code is required");
        }

        if (seatCount <= 0)
        {
            throw new ArgumentException("Seat count must be positive");
        }

        if (string.IsNullOrWhiteSpace(paymentMethod))
        {
            throw new ArgumentException("Payment method is required");
        }
    }

    public static (Customer customer, SubscriptionPlan plan) RetrieveAndValidateCustomer(int customerId, string normalizedPlanCode)
    {
        var customerRepository = new CustomerRepository();
        var planRepository = new SubscriptionPlanRepository();
        
        var customer = customerRepository.GetById(customerId);
        var plan = planRepository.GetByCode(normalizedPlanCode);
        
        if (!customer.IsActive)
        {
            throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
        }

        return new(customer, plan);
    }
}