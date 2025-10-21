using SchedulerApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchedulerApp.Models.Entities
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Organization { get; set; }
        public string? Phone { get; set; }

    }
}