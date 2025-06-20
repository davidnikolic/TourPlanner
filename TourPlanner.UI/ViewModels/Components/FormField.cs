using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.UI.ViewModels.Components
{
    public class FormField
    { 
        public string Label { get; set; } = "";

        public string Type { get; set; } = "TextBox";

        public object? Value { get; set; }

        public IEnumerable<object>? Options { get; set; }
    }
}
