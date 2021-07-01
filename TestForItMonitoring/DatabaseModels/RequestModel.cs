using System.ComponentModel.DataAnnotations;

namespace TestForItMonitoring.DatabaseModels
{
    public class RequestModel
    {
        [Key]
        public int Id { get; set; }
        public string ClientIp { get; set; }
        public string FirstNumber { get; set; }
        public string SecondNumber { get; set; }
        public string Result { get; set; }
    }
}
