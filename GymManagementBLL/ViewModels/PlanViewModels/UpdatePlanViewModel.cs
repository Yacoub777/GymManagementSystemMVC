using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    { 
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage ="Duration Days Are Required")]
        [Range(1,365, ErrorMessage ="Duration Days Must Be Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage ="Price Is Required")]
        [Range(0.1, 10000, ErrorMessage ="Price Must Be Between 0.1 and 10000")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(200,MinimumLength =5, ErrorMessage = "Description Must Be Between 5 and 200 Char")]

        public string Description { get; set; } = null!;

    }
}
