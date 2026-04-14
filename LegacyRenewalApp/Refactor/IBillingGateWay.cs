namespace LegacyRenewalApp.Refactor;

public interface IBillingGateWay
{
    public void SaveInvoice(RenewalInvoice invoice);
    public void SendNotification(string target, string subject, string body);
}