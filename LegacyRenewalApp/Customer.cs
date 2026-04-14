using LegacyRenewalApp.Refactor;

namespace LegacyRenewalApp
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Segment { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int YearsWithCompany { get; set; }
        public int LoyaltyPoints { get; set; }
        public bool IsActive { get; set; }

        public void ApplyDiscountBasedOnPlan(Discount discount, SubscriptionPlan plan)
        {
            switch (this.Segment)
            {
                case "Silver":
                    discount.AddDisc(0.05m, "silver discount; ");
                    break;
                case "Gold":
                    discount.AddDisc(0.10m, "gold discount; ");
                    break;
                case "Platinum":
                    discount.AddDisc(0.15m, "platinum discount; ");
                    break;
                case "Education":
                    if (plan.IsEducationEligible)
                        discount.AddDisc(0.20m, "education discount; ");
                    break;
            }
        }

        public void ApplyDicountBasedOnYearsWithCompany(Discount discount)
        {
            if (this.YearsWithCompany >= 5)
            {
                discount.AddDisc(0.07m, "long-term loyalty discount; ");
            }
            else if (this.YearsWithCompany >= 2)
            {
                discount.AddDisc(0.03m, "basic loyalty discount; ");
            }
        }
        public void ApplyDiscountBasedOnLoyaltyPoints(Discount discount, bool useLoyaltyPoints)
        {
            if (useLoyaltyPoints && this.LoyaltyPoints > 0)
            {
                int pointsToUse = this.LoyaltyPoints > 200 ? 200 : this.LoyaltyPoints;
                discount.discountAmount += pointsToUse;
                discount.notes += $"loyalty points used: {pointsToUse}; ";
            }
        }

        public decimal GetTaxRateOfCustomerCountry()
        {
            decimal taxRate;
            switch (this.Country)
            {
                case "Poland": 
                    taxRate = 0.23m;
                    break;
                case "Germany":
                    taxRate = 0.19m;
                    break;
                case "Czech Republic":
                    taxRate = 0.21m;
                    break;
                case "Norway":
                    taxRate = 0.25m;
                    break;
                default:
                    taxRate = 0.20m;
                    break;
            }
            return taxRate;
        }
    }
}
