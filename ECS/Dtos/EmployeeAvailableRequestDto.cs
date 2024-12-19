namespace ECS.Dtos
{
    public class EmployeeAvailableRequestDto
    {
        public Guid ProductId { get; set; }
        public int RequiredEmployees { get; set; }
    }
}
