namespace DentalHealthSaaS.Backend.src.Application.DTOs.Payments
{
    public class CreatePaymentDto
    {
        public Guid VisitId { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
    }
}
