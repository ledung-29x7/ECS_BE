using System.ComponentModel.DataAnnotations;

namespace ECS.Areas.Authen.Models
{
    public class Role
    {
        private int roleId;
        private string roleName;
        private decimal baseSalary;

        [Key]
        public int RoleId { get => roleId; set => roleId = value; }
        public string RoleName { get => roleName; set => roleName = value; }
        public decimal BaseSalary { get => baseSalary; set => baseSalary = value; }
    }
}
