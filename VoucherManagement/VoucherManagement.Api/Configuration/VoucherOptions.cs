namespace VoucherManagement.Api.Configuration
{
    public class VoucherOptions
    {
        public string TemplatePath { get; set; } = string.Empty;

        public int PdfPage { get; set; } = 1;
        public float QrCodeSize { get; set; } = 60;
        public int FontSize { get; set; } = 5;
        public int SpacingRight { get; set; } = 45;
        public int SpacingBottom { get; set; } = 40;
        public int TextSpacingBottom { get; set; } = 45;
    }
}
