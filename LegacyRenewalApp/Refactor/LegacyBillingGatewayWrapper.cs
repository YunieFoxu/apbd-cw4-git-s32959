namespace LegacyRenewalApp.Refactor;

public class LegacyBillingGatewayWrapper : IBillingGateWay
{
    public void SaveInvoice(RenewalInvoice invoice)
    {
        LegacyBillingGateway.SaveInvoice(invoice);
    }

    public void SendNotification(string target, string subject, string body)
    {
        LegacyBillingGateway.SendEmail(target, subject, body);
    }
}