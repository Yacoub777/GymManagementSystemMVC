using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int DurationDays { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
