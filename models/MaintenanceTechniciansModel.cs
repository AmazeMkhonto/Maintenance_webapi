
using System.ComponentModel.DataAnnotations;

public class MaintenanceTechnician
{
    public int TechnicianID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public int ContactID { get; set; }
    public bool is_deleted { get; set; }


}
