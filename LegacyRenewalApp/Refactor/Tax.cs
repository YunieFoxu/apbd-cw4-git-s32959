namespace LegacyRenewalApp.Refactor;

public class Tax
{
    public decimal taxRate { get; set; }
    public decimal taxBase { get; set; }
    public decimal taxAmount { get; set; }
    public decimal finalAmount { get; set; }
    public Tax(Customer customer, Discount discount)
    {
        taxRate = customer.GetTaxRateOfCustomerCountry();
        taxBase = discount.subtotalAfterDiscount + discount.supportFee + discount.paymentFee;
        taxAmount = taxBase * taxRate;
        finalAmount = taxBase + taxAmount;
        
        if (finalAmount < 500m)
        {
            finalAmount = 500m;
            discount.notes += "minimum invoice amount applied; ";
        }
    }
}