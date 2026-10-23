namespace QR
{
    internal record QrData
    {
        public Version Version { get; init; }
        public ErrorCorrectionLevel CorrectionLevel { get; init; }
        public string Data { get; init; }
    }
}
