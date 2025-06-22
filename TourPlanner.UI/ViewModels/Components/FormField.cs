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

        private string? _error;
        public string? Error
        {
            get => _error;
            set { _error = value; }
        }

        private object? _value;
        public object? Value
        {
            get => _value;
            set { _value = value; }
        }


        public IEnumerable<object>? Options { get; set; }

        public bool IsValid => string.IsNullOrEmpty(Error);
    }
}
