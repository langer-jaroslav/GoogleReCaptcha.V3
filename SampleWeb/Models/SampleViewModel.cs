using System.ComponentModel;

namespace SampleWeb.Models
{
    public class SampleViewModel
    {
        [DisplayName("Sample textbox")]
        public string Text { get; set; }
    }
}
