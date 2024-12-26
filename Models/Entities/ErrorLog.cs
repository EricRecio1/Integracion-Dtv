namespace IntegracionOcasaDtv.Models.Entities
{
    public class ErrorLog
    {
        public Message Message { get; set; }
        public Integration Integration { get; set; }
        public string Country { get; set; }
        public string DocumentType { get; set; }
        public string DocumentId { get; set; }
        public ProcessData ProcessData { get; set; }
    }

    public class ProcessData
    {
        public string Entity { get; set; }
        public string Value { get; set; }
        public string ProcessDate { get; set; }
        public ErrorDetail ErrorDetail { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string ErrorDate { get; set; }
    }
}
