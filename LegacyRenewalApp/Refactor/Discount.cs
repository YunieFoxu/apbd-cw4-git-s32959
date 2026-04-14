using System;

namespace LegacyRenewalApp.Refactor;

public class Discount
{
    public decimal baseAmount { get; set; }
    public decimal discountAmount { get; set; }
    public string notes { get; set; }
    
    public decimal subtotalAfterDiscount { get; set; }
    
    public decimal supportFee { get; set; }
    
    public decimal paymentFee { get; set; }
    public Discount(decimal BaseAmount)
    {
        baseAmount = BaseAmount;
        discountAmount = 0m;
        notes = string.Empty;
        supportFee = 0m;
        paymentFee = 0m;
    }
    
    public void AddDisc(decimal mult)
    {
        this.discountAmount += baseAmount * mult;
    }
    public void AddDisc(decimal mult, string note)
    {
        this.discountAmount += baseAmount * mult;
        this.notes += note;
    }

    public void CalculateSubtotal()
    {
       subtotalAfterDiscount = baseAmount - discountAmount;
        if (subtotalAfterDiscount < 300m)
        {
            subtotalAfterDiscount = 300m;
            notes += "minimum discounted subtotal applied; ";
        }
    }

    public void CalculateSupportFee(bool includePremiumSupport, string normalizedPlanCode)
    {
        if (includePremiumSupport)
        {
            if (normalizedPlanCode == "START")
            {
                supportFee = 250m;
            }
            else if (normalizedPlanCode == "PRO")
            {
                supportFee = 400m;
            }
            else if (normalizedPlanCode == "ENTERPRISE")
            {
                supportFee = 700m;
            }

            notes += "premium support included; ";
        }
    }

    public void CalculatePaymentFee(string normalizedPaymentMethod)
    {
        if (normalizedPaymentMethod == "CARD")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.02m;
            notes += "card payment fee; ";
        }
        else if (normalizedPaymentMethod == "BANK_TRANSFER")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.01m;
            notes += "bank transfer fee; ";
        }
        else if (normalizedPaymentMethod == "PAYPAL")
        {
            paymentFee = (subtotalAfterDiscount + supportFee) * 0.035m;
            notes += "paypal fee; ";
        }
        else if (normalizedPaymentMethod == "INVOICE")
        {
            paymentFee = 0m;
            notes += "invoice payment; ";
        }
        else
        {
            throw new ArgumentException("Unsupported payment method");
        }
    }
}