public class MaintenanceRequest
{
    public int RequestID { get; set; }
    public int TenantID { get; set; }
    public string Description { get; set; }
    public DateTime RequestDate { get; set; }
    public byte[] Image { get; set; }
    public bool is_deleted { get; set; }
}
