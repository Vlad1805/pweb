namespace MobyLabWebProgramming.Infrastructure.Configurations;

public class MailConfiguration
{
    public bool MailEnable { get; set; }
    public string MailHost { get; set; } = default!;
    public ushort MailPort { get; set; }
    public string MailAddress { get; set; } = default!;
    public string MailUser { get; set; } = default!;
    public string MailPassword { get; set; } = default!;
}
